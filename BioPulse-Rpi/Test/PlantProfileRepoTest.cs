using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    public class PlantProfileRepoTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly AppDbContext _dbContext;
        private readonly PlantProfileRepo _plantProfileRepo;

        public PlantProfileRepoTests()
        {
            // Set up DbContext with in-memory database for testing
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("PlantProfileTestDatabase"));

            // Register repository using AppDbContext
            serviceCollection.AddScoped<PlantProfileRepo>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
            _plantProfileRepo = _serviceProvider.GetRequiredService<PlantProfileRepo>();
        }

        [Fact]
        public async Task CanAddPlantProfileToDatabase()
        {
            // Arrange
            var plantProfile = new PlantProfile
            {
                Name = "Tomato",
                IsDefault = false,
                PhMin = 5.5,
                PhMax = 6.5,
                TemperatureMin = 18.0,
                TemperatureMax = 26.0,
                LightOnTime = new TimeSpan(6, 0, 0),
                LightOffTime = new TimeSpan(18, 0, 0),
                LightMin = 5000,
                LightMax = 70000,
                EcMin = 1.5,
                EcMax = 2.5
            };

            // Act
            await _plantProfileRepo.AddAsync(plantProfile);
            var result = await _plantProfileRepo.GetByIdAsync(plantProfile.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tomato", result.Name);
        }

        [Fact]
        public async Task CanUpdatePlantProfile()
        {
            // Arrange
            var plantProfile = new PlantProfile
            {
                Name = "Lettuce",
                IsDefault = true,
                PhMin = 6.0,
                PhMax = 7.0,
                TemperatureMin = 15.0,
                TemperatureMax = 22.0,
                LightOnTime = new TimeSpan(7, 0, 0),
                LightOffTime = new TimeSpan(19, 0, 0),
                LightMin = 8000,
                LightMax = 60000,
                EcMin = 1.0,
                EcMax = 2.0
            };
            await _plantProfileRepo.AddAsync(plantProfile);

            // Act
            plantProfile.IsDefault = false;
            plantProfile.LightMax = 65000;
            await _plantProfileRepo.UpdateAsync(plantProfile);
            var result = await _plantProfileRepo.GetByIdAsync(plantProfile.Id);

            // Assert
            Assert.False(result.IsDefault);
            Assert.Equal(65000, result.LightMax);
        }

        [Fact]
        public async Task CanDeletePlantProfile()
        {
            // Arrange
            var plantProfile = new PlantProfile
            {
                Name = "Basil",
                IsDefault = false,
                PhMin = 5.8,
                PhMax = 6.8,
                TemperatureMin = 20.0,
                TemperatureMax = 28.0,
                LightOnTime = new TimeSpan(5, 0, 0),
                LightOffTime = new TimeSpan(17, 0, 0),
                LightMin = 4000,
                LightMax = 60000,
                EcMin = 1.2,
                EcMax = 2.2
            };
            await _plantProfileRepo.AddAsync(plantProfile);

            // Act
            await _plantProfileRepo.DeleteAsync(plantProfile.Id);
            var result = await _plantProfileRepo.GetByIdAsync(plantProfile.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CanRetrieveAllPlantProfiles()
        {
            // Arrange
            var plantProfile1 = new PlantProfile
            {
                Name = "Mint",
                IsDefault = true,
                PhMin = 5.5,
                PhMax = 6.5,
                TemperatureMin = 16.0,
                TemperatureMax = 24.0,
                LightOnTime = new TimeSpan(6, 0, 0),
                LightOffTime = new TimeSpan(18, 0, 0),
                LightMin = 3000,
                LightMax = 50000,
                EcMin = 1.0,
                EcMax = 2.0
            };

            var plantProfile2 = new PlantProfile
            {
                Name = "Cucumber",
                IsDefault = false,
                PhMin = 6.0,
                PhMax = 7.0,
                TemperatureMin = 20.0,
                TemperatureMax = 30.0,
                LightOnTime = new TimeSpan(7, 0, 0),
                LightOffTime = new TimeSpan(19, 0, 0),
                LightMin = 5000,
                LightMax = 70000,
                EcMin = 1.5,
                EcMax = 2.5
            };

            await _plantProfileRepo.AddAsync(plantProfile1);
            await _plantProfileRepo.AddAsync(plantProfile2);

            // Act
            var result = await _plantProfileRepo.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
