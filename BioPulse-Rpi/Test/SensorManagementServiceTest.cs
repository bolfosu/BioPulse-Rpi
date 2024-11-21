using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DataAccessLayer;
using LogicLayer; 
 

public class SensorManagementServiceTest
{
    private readonly ServiceProvider _serviceProvider;
    private readonly SensorManagementService _sensorManagementService;

    public SensorManagementServiceTest()
    {
        var serviceCollection = new ServiceCollection();

        // Register DbContext with an in-memory database
        serviceCollection.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase_SensorManagement"));

        // Register repositories
        serviceCollection.AddScoped<EcSensorRepo>();
        serviceCollection.AddScoped<PhSensorRepo>();
        serviceCollection.AddScoped<LightSensorRepo>();
        serviceCollection.AddScoped<TemperatureSensorRepo>(); 

        // Register SensorManagementService
        serviceCollection.AddScoped<SensorManagementService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _sensorManagementService = _serviceProvider.GetRequiredService<SensorManagementService>();
    }


    [Fact]
    public async Task CanAddAndRetrieveEcSensor()
    {
        var ecSensor = new EcSensor { Name = "EC Sensor 1", IsEnabled = true, LastReading = 4.4 };
        await _sensorManagementService.AddEcSensorAsync(ecSensor);

        var result = await _sensorManagementService.GetEcSensorAsync(ecSensor.Id);
        Assert.NotNull(result);
        Assert.Equal("EC Sensor 1", result.Name);
    }

  

    
}
