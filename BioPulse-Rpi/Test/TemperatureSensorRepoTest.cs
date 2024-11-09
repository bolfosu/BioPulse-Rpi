using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TemperatureSensorRepoTest
    {
        private readonly IServiceProvider _serviceProvider;

        public TemperatureSensorRepoTest()
        {
            // Set up DbContext with in-memory database for testing
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            // Register repository using AppDbContext
            serviceCollection.AddScoped<TemperatureSensorRepo>();
            serviceCollection.AddScoped<IRepository<TemperatureSensor>, TemperatureSensorRepo>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private AppDbContext GetDbContext()
        {
            return _serviceProvider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task CanAddTemperatureSensorToDatabase()
        {
            // Arrange
            var repository = _serviceProvider.GetRequiredService<IRepository<TemperatureSensor>>();

            var newSensor = new TemperatureSensor
            {
                Name = "Temp Sensor 1",
                IsEnabled = true,
                IsWireless = false,
                LastReading = 22.5,
                LastReadingTime = DateTime.Now,
                Address = 1
            };

            // Act: Add the sensor to the database
            await repository.AddAsync(newSensor);

            // Assert: Verify the sensor was added
            var dbContext = GetDbContext();
            var addedSensor = await dbContext.TemperatureSensors.FirstOrDefaultAsync(s => s.Name == "Temp Sensor 1");

            Assert.NotNull(addedSensor);
            Assert.Equal("Temp Sensor 1", addedSensor.Name);
            Assert.Equal(22.5, addedSensor.LastReading);
        }
    }
}
