using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;

namespace LogicLayer.Services
{
    public class SensorDataIngestionService
    {
        private readonly IRepository<Sensor> _sensorRepo;
        private readonly IRepository<PlantProfile> _plantProfileRepo;
        private readonly ActuatorService _actuatorService;
        private readonly ILogger<SensorDataIngestionService> _logger;
        private readonly Dictionary<string, int> _sensorCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public SensorDataIngestionService(
            IRepository<Sensor> sensorRepo,
            IRepository<PlantProfile> plantProfileRepo,
            ActuatorService actuatorService,
            ILogger<SensorDataIngestionService> logger)
        {
            _sensorRepo = sensorRepo;
            _plantProfileRepo = plantProfileRepo;
            _actuatorService = actuatorService;
            _logger = logger;

            InitializeCache().Wait();
        }

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
            await _semaphore.WaitAsync();
            try
            {
                var readingData = JsonSerializer.Deserialize<SensorReadingDto>(jsonReading);
                if (readingData == null) return;

                var plantProfiles = await _plantProfileRepo.GetAllAsync();
                var defaultProfile = plantProfiles.FirstOrDefault(p => p.IsDefault);

                if (defaultProfile != null && Enum.TryParse<SensorType>(readingData.TYPE, true, out var sensorType))
                {
                    switch (sensorType)
                    {
                        case SensorType.Temp:
                            if (readingData.VALUE > defaultProfile.TemperatureMax)
                            {
                                _logger.LogInformation("Temperature exceeded max threshold. Switching actuator ON.");
                                await _actuatorService.SwitchOnAsync(1); // Example actuator ID
                            }
                            else if (readingData.VALUE < defaultProfile.TemperatureMin)
                            {
                                _logger.LogInformation("Temperature below min threshold. Switching actuator OFF.");
                                await _actuatorService.SwitchOffAsync(1);
                            }
                            break;

                        case SensorType.EC:
                            if (readingData.VALUE > defaultProfile.EcMax)
                            {
                                _logger.LogInformation("EC exceeded max threshold. Switching actuator ON.");
                                await _actuatorService.SwitchOnAsync(2); // Example actuator ID
                            }
                            else if (readingData.VALUE < defaultProfile.EcMin)
                            {
                                _logger.LogInformation("EC below min threshold. Switching actuator OFF.");
                                await _actuatorService.SwitchOffAsync(2);
                            }
                            break;

                        default:
                            _logger.LogInformation("Sensor type '{SensorType}' does not have actuator logic implemented.", sensorType);
                            break;
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to process sensor reading JSON: {Json}", jsonReading);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private class SensorReadingDto
        {
            [JsonPropertyName("id")]
            public string SENSOR_ID { get; set; }

            [JsonPropertyName("type")]
            public string TYPE { get; set; }

            [JsonPropertyName("Temp")]
            public double VALUE { get; set; }
        }
    }
}
