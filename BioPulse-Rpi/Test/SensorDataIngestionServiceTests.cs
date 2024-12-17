using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LogicLayer.Tests
{
    public class SensorDataIngestionServiceTests
    {
        private readonly Mock<IRepository<Sensor>> _mockSensorRepo;
        private readonly Mock<IRepository<PlantProfile>> _mockPlantProfileRepo;
        private readonly Mock<ActuatorService> _mockActuatorService;
        private readonly Mock<ILogger<SensorDataIngestionService>> _mockLogger;
        private readonly SensorDataIngestionService _service;

        public SensorDataIngestionServiceTests()
        {
            _mockSensorRepo = new Mock<IRepository<Sensor>>();
            _mockPlantProfileRepo = new Mock<IRepository<PlantProfile>>();
            _mockActuatorService = new Mock<ActuatorService>(null, null, 0x60);
            _mockLogger = new Mock<ILogger<SensorDataIngestionService>>();

            _service = new SensorDataIngestionService(
                _mockSensorRepo.Object,
                _mockPlantProfileRepo.Object,
                _mockActuatorService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessReadingAsync_TemperatureAboveMax_TriggersActuatorOn()
        {
            // Arrange
            var plantProfiles = new List<PlantProfile>
            {
                new PlantProfile
                {
                    Id = 1,
                    Name = "Test Profile",
                    Active = true,
                    TemperatureMax = 25.0,
                    TemperatureMin = 20.0
                }
            };

            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(plantProfiles);

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "Temp",
                Temp = 30.0 // Above max threshold
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(1), Times.Once); // Actuator ID 1 is triggered
            _mockActuatorService.Verify(service => service.SwitchOffAsync(It.IsAny<int>()), Times.Never); // No OFF call
        }

        [Fact]
        public async Task ProcessReadingAsync_TemperatureWithinRange_DoesNotTriggerActuator()
        {
            // Arrange
            var plantProfiles = new List<PlantProfile>
            {
                new PlantProfile
                {
                    Id = 1,
                    Name = "Test Profile",
                    Active = true,
                    TemperatureMax = 25.0,
                    TemperatureMin = 20.0
                }
            };

            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(plantProfiles);

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "Temp",
                Temp = 22.0 // Within range
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(It.IsAny<int>()), Times.Never); // No ON call
            _mockActuatorService.Verify(service => service.SwitchOffAsync(It.IsAny<int>()), Times.Never); // No OFF call
        }

        [Fact]
        public async Task ProcessReadingAsync_TemperatureBelowMin_TriggersActuatorOn()
        {
            // Arrange
            var plantProfiles = new List<PlantProfile>
            {
                new PlantProfile
                {
                    Id = 1,
                    Name = "Test Profile",
                    Active = true,
                    TemperatureMax = 25.0,
                    TemperatureMin = 20.0
                }
            };

            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(plantProfiles);

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "Temp",
                Temp = 18.0 // Below min threshold
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(1), Times.Once); // Actuator ID 1 is triggered
            _mockActuatorService.Verify(service => service.SwitchOffAsync(It.IsAny<int>()), Times.Never); // No OFF call
        }

        [Fact]
        public async Task ProcessReadingAsync_NoActiveProfile_DoesNotProcessReading()
        {
            // Arrange
            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<PlantProfile>()); // No active profile

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "Temp",
                Temp = 30.0 // Above max threshold
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(It.IsAny<int>()), Times.Never); // No ON call
            _mockActuatorService.Verify(service => service.SwitchOffAsync(It.IsAny<int>()), Times.Never); // No OFF call
        }

        [Fact]
        public async Task ProcessReadingAsync_EcAboveMax_TriggersEcDownActuator()
        {
            // Arrange
            var plantProfiles = new List<PlantProfile>
            {
                new PlantProfile
                {
                    Id = 1,
                    Name = "Test Profile",
                    Active = true,
                    EcMax = 2.0,
                    EcMin = 1.0
                }
            };

            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(plantProfiles);

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "EC",
                Temp = 2.5 // Above max EC
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(2), Times.Once); // EcDown actuator ID
        }

        [Fact]
        public async Task ProcessReadingAsync_EcBelowMin_TriggersEcUpActuator()
        {
            // Arrange
            var plantProfiles = new List<PlantProfile>
            {
                new PlantProfile
                {
                    Id = 1,
                    Name = "Test Profile",
                    Active = true,
                    EcMax = 2.0,
                    EcMin = 1.0
                }
            };

            _mockPlantProfileRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(plantProfiles);

            var jsonReading = JsonSerializer.Serialize(new
            {
                id = "1",
                type = "EC",
                Temp = 0.5 // Below min EC
            });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(3), Times.Once); // EcUp actuator ID
        }
    }
}
