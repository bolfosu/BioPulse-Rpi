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

        /// <summary>
        /// Initializes the sensor cache with known sensors.
        /// </summary>
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

        /// <summary>
        /// Processes a JSON reading from a sensor.
        /// </summary>
        /// <param name="jsonReading">The JSON string containing the sensor reading.</param>
        public async Task ProcessReadingAsync(string jsonReading)
        {
            await _semaphore.WaitAsync();
            try
            {
                // Deserialize and validate the JSON reading
                var readingData = JsonSerializer.Deserialize<SensorReadingDto>(jsonReading);
                if (readingData == null)
                {
                    _logger.LogWarning("Failed to parse sensor reading JSON.");
                    return;
                }

                if (!Enum.TryParse<SensorType>(readingData.TYPE, true, out var sensorType))
                {
                    _logger.LogWarning("Invalid sensor type '{TYPE}' in reading.", readingData.TYPE);
                    return;
                }

                // Retrieve the active plant profile
                var plantProfiles = await _plantProfileRepo.GetAllAsync();
                var activeProfile = plantProfiles.FirstOrDefault(p => p.Active);

                if (activeProfile == null)
                {
                    _logger.LogInformation("No active plant profile found. Skipping sensor reading processing.");
                    return;
                }

                // Process the sensor reading based on its type
                await ProcessSensorReading(readingData, sensorType, activeProfile);
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

        private async Task ProcessSensorReading(SensorReadingDto readingData, SensorType sensorType, PlantProfile activeProfile)
        {
            switch (sensorType)
            {
                case SensorType.Temp:
                    await HandleTemperatureReading(readingData.VALUE, activeProfile);
                    break;

                case SensorType.EC:
                    await HandleEcReading(readingData.VALUE, activeProfile);
                    break;

                default:
                    _logger.LogInformation("Sensor type '{SensorType}' does not have actuator logic implemented.", sensorType);
                    break;
            }
        }

        private async Task HandleTemperatureReading(double value, PlantProfile profile)
        {
            if (value > profile.TemperatureMax)
            {
                _logger.LogInformation("Temperature exceeded max threshold. Switching actuator ON.");
                await _actuatorService.SwitchOnAsync(1); // Example actuator ID for temperature control
            }
            else if (value < profile.TemperatureMin)
            {
                _logger.LogInformation("Temperature below min threshold. Switching actuator ON.");
                await _actuatorService.SwitchOnAsync(1); // Example actuator ID for temperature control
            }
            else
            {
                _logger.LogInformation("Temperature within range. Switching actuator OFF.");
                await _actuatorService.SwitchOffAsync(1); // Example actuator ID for temperature control
            }
        }

        private async Task HandleEcReading(double value, PlantProfile profile)
        {
            if (value > profile.EcMax)
            {
                _logger.LogInformation("EC exceeded max threshold. Switching EcDown actuator ON.");
                await _actuatorService.SwitchOnAsync(2); // EcDown actuator ID
            }
            else if (value < profile.EcMin)
            {
                _logger.LogInformation("EC below min threshold. Switching EcUp actuator ON.");
                await _actuatorService.SwitchOnAsync(3); // EcUp actuator ID
            }
            else
            {
                _logger.LogInformation("EC within range. Switching actuators OFF.");
                await _actuatorService.SwitchOffAsync(2); // EcDown actuator ID
                await _actuatorService.SwitchOffAsync(3); // EcUp actuator ID
            }
        }

        /// <summary>
        /// DTO for sensor reading deserialization.
        /// </summary>
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
