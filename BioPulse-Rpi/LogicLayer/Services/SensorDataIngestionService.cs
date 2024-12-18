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
        private readonly IRepository<PlantProfile> _plantProfileRepo;
        private readonly ActuatorService _actuatorService;
        private readonly ILogger<SensorDataIngestionService> _logger;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public SensorDataIngestionService(
            IRepository<PlantProfile> plantProfileRepo,
            ActuatorService actuatorService,
            ILogger<SensorDataIngestionService> logger)
        {
            _plantProfileRepo = plantProfileRepo;
            _actuatorService = actuatorService;
            _logger = logger;
        }

        /// <summary>
        /// Processes a JSON reading from a sensor.
        /// </summary>
        public async Task ProcessReadingAsync(string jsonReading)
        {
            await _semaphore.WaitAsync();
            try
            {
                var readingData = JsonSerializer.Deserialize<SensorReadingDto>(jsonReading);
                if (readingData == null)
                {
                    _logger.LogWarning("Failed to parse sensor reading JSON.");
                    return;
                }

                // Validate sensor type
                if (!Enum.TryParse<SensorType>(readingData.TYPE, true, out var sensorType) || sensorType != SensorType.pH)
                {
                    _logger.LogInformation("Only pH sensor readings are processed.");
                    return;
                }

                // Retrieve active profile
                var profiles = await _plantProfileRepo.GetAllAsync();
                var activeProfile = profiles.FirstOrDefault(p => p.Active);

                if (activeProfile == null)
                {
                    _logger.LogWarning("No active plant profile found. Skipping pH processing.");
                    return;
                }

                // Handle pH reading
                await HandlePhReading(readingData.VALUE, activeProfile);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize sensor reading JSON: {Json}", jsonReading);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task HandlePhReading(double value, PlantProfile profile)
        {
            if (value < profile.PhMin)
            {
                _logger.LogInformation("pH is below minimum threshold ({Min}). Triggering pH UP actuator.", profile.PhMin);
                await _actuatorService.SwitchOnAsync(); // pH Up actuator
            }
            else if (value > profile.PhMax)
            {
                _logger.LogInformation("pH is above maximum threshold ({Max}). Triggering pH UP actuator.", profile.PhMax);
                await _actuatorService.SwitchOnAsync(); // pH Up actuator
            }
            else
            {
                _logger.LogInformation("pH is within acceptable range ({Min}-{Max}). Switching actuator OFF.", profile.PhMin, profile.PhMax);
                await _actuatorService.SwitchOffAsync();
            }
        }


        /// <summary>
        /// DTO for deserializing sensor reading JSON.
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