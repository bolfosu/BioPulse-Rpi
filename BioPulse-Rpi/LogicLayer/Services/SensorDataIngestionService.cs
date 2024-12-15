using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LogicLayer.Services
{
    public class SensorDataIngestionService
    {
        private readonly IRepository<Sensor> _sensorRepo;
        private readonly IRepository<SensorReading> _sensorReadingRepo;
        private readonly ILogger<SensorDataIngestionService> _logger;

        private readonly Dictionary<string, int> _sensorCache = new(StringComparer.OrdinalIgnoreCase); // Cache for known sensors
        private readonly SemaphoreSlim _semaphore = new(1, 1); // Semaphore for thread safety

        public SensorDataIngestionService(IRepository<Sensor> sensorRepo, IRepository<SensorReading> sensorReadingRepo, ILogger<SensorDataIngestionService> logger)
        {
            _sensorRepo = sensorRepo;
            _sensorReadingRepo = sensorReadingRepo;
            _logger = logger;

            InitializeCache().Wait(); // Initialize cache synchronously during startup
        }

        // Initialize sensor cache
        private async Task InitializeCache()
        {
            _logger.LogInformation("Loading sensors into cache...");
            var sensors = await _sensorRepo.GetAllAsync();
            foreach (var sensor in sensors)
            {
                if (!string.IsNullOrEmpty(sensor.ExternalSensorId))
                {
                    _sensorCache[sensor.ExternalSensorId] = sensor.Id;
                }
            }
            _logger.LogInformation("Sensor cache initialized with {Count} entries.", _sensorCache.Count);
        }

        public async Task ProcessReadingAsync(string jsonReading)
        {
            await _semaphore.WaitAsync(); // Ensure only one thread processes at a time
            try
            {
                // Deserialize JSON into a DTO
                var readingData = JsonSerializer.Deserialize<SensorReadingDto>(jsonReading);
                if (readingData == null)
                {
                    _logger.LogWarning("Failed to parse sensor reading JSON.");
                    return;
                }

                // Validate sensor type
                if (!Enum.TryParse<SensorType>(readingData.TYPE, out var sensorType))
                {
                    _logger.LogWarning("Invalid sensor type '{TYPE}' in reading.", readingData.TYPE);
                    return;
                }

                // Check if sensor is known
                if (!_sensorCache.TryGetValue(readingData.SENSOR_ID, out int sensorId))
                {
                    // Add new sensor if not known
                    var newSensor = new Sensor
                    {
                        Name = $"Sensor_{readingData.SENSOR_ID}",
                        ExternalSensorId = readingData.SENSOR_ID,
                        SensorType = sensorType,
                        ConnectionDetails = $"Generated at {DateTime.UtcNow}"
                    };

                    await _sensorRepo.AddAsync(newSensor);
                    sensorId = newSensor.Id;
                    _sensorCache[readingData.SENSOR_ID] = sensorId;
                    _logger.LogInformation("New sensor created with ID {SensorId}.", sensorId);
                }

                // Create sensor reading
                var sensorReading = new SensorReading
                {
                    SensorId = sensorId,
                    Timestamp = readingData.TIMESTAMP,
                    Value = readingData.VALUE
                };

                await _sensorReadingRepo.AddAsync(sensorReading);
                _logger.LogInformation("Stored reading for SensorId {SensorId}: Value={Value}, Timestamp={Timestamp}",
                    sensorId, readingData.VALUE, readingData.TIMESTAMP);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing sensor reading JSON: {Json}", jsonReading);
            }
            finally
            {
                _semaphore.Release(); // Release semaphore lock
            }
        }

        // DTO for deserializing sensor reading JSON
        private class SensorReadingDto
        {
            public string SENSOR_ID { get; set; } // Sensor's unique identifier
            public string TYPE { get; set; } // Sensor type
            public double VALUE { get; set; } // Sensor reading value
            public DateTime TIMESTAMP { get; set; } // Timestamp of the reading
        }
    }
}
