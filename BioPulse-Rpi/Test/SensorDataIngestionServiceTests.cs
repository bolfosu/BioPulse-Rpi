using System.Threading.Tasks;
using Xunit;
using Moq;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.Extensions.Logging;

namespace LogicLayer.Tests
{
    public class SensorDataIngestionServiceTests
    {
        private readonly Mock<IRepository<PlantProfile>> _mockPlantProfileRepo;
        private readonly Mock<ActuatorService> _mockActuatorService;
        private readonly Mock<ILogger<SensorDataIngestionService>> _mockLogger;
        private readonly SensorDataIngestionService _service;

        public SensorDataIngestionServiceTests()
        {
            _mockPlantProfileRepo = new Mock<IRepository<PlantProfile>>();
            _mockActuatorService = new Mock<ActuatorService>(MockBehavior.Strict, null, 0x59);
            _mockLogger = new Mock<ILogger<SensorDataIngestionService>>();

            _service = new SensorDataIngestionService(
                _mockPlantProfileRepo.Object,
                _mockActuatorService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task ProcessReadingAsync_PhBelowMin_TriggersPhUpActuator()
        {
            // Arrange
            var jsonReading = "{\"id\":\"sensor_001\",\"type\":\"pH\",\"Temp\":5.5}";
            var activeProfile = new PlantProfile
            {
                Id = 1,
                Name = "Test Profile",
                Active = true,
                PhMin = 6.0,
                PhMax = 7.5
            };

            _mockPlantProfileRepo
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new[] { activeProfile });

            _mockActuatorService
                .Setup(service => service.SwitchOnAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(), Times.Once);
            _mockActuatorService.Verify(service => service.SwitchOffAsync(), Times.Never);
        }

        [Fact]
        public async Task ProcessReadingAsync_PhWithinRange_TriggersActuatorOff()
        {
            // Arrange
            var jsonReading = "{\"id\":\"sensor_001\",\"type\":\"pH\",\"Temp\":6.5}";
            var activeProfile = new PlantProfile
            {
                Id = 1,
                Name = "Test Profile",
                Active = true,
                PhMin = 6.0,
                PhMax = 7.5
            };

            _mockPlantProfileRepo
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new[] { activeProfile });

            _mockActuatorService
                .Setup(service => service.SwitchOffAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOffAsync(), Times.Once);
            _mockActuatorService.Verify(service => service.SwitchOnAsync(), Times.Never);
        }

        [Fact]
        public async Task ProcessReadingAsync_NoActiveProfile_SkipsProcessing()
        {
            // Arrange
            var jsonReading = "{\"id\":\"sensor_001\",\"type\":\"pH\",\"Temp\":5.0}";
            _mockPlantProfileRepo
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new PlantProfile[] { }); // No active profile

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(), Times.Never);
            _mockActuatorService.Verify(service => service.SwitchOffAsync(), Times.Never);
        }

        [Fact]
        public async Task ProcessReadingAsync_InvalidSensorType_SkipsProcessing()
        {
            // Arrange
            var jsonReading = "{\"id\":\"sensor_001\",\"type\":\"EC\",\"Temp\":5.5}";
            var activeProfile = new PlantProfile
            {
                Id = 1,
                Name = "Test Profile",
                Active = true,
                PhMin = 6.0,
                PhMax = 7.5
            };

            _mockPlantProfileRepo
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new[] { activeProfile });

            // Act
            await _service.ProcessReadingAsync(jsonReading);

            // Assert
            _mockActuatorService.Verify(service => service.SwitchOnAsync(), Times.Never);
            _mockActuatorService.Verify(service => service.SwitchOffAsync(), Times.Never);
        }

        [Fact]
        public async Task ProcessReadingAsync_InvalidJson_DoesNotThrow()
        {
            // Arrange
            var invalidJsonReading = "{\"id\":\"sensor_001\",\"type\":\"pH\""; // Incomplete JSON
            _mockPlantProfileRepo
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new PlantProfile[] { });

            // Act & Assert
            await _service.ProcessReadingAsync(invalidJsonReading);

            _mockActuatorService.Verify(service => service.SwitchOnAsync(), Times.Never);
            _mockActuatorService.Verify(service => service.SwitchOffAsync(), Times.Never);
        }
    }
}
