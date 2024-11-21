using Xunit;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using DataAccessLayer;

public class EcSensorRepoTest
{
    private readonly ServiceProvider _serviceProvider;

    public EcSensorRepoTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase_EcSensor"));
        serviceCollection.AddScoped<EcSensorRepo>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task CanAddEcSensorToDatabase()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<EcSensorRepo>();
            var ecSensor = new EcSensor { Name = "Test EC Sensor", IsEnabled = true, LastReading = 1.5 };
            await repo.AddAsync(ecSensor);

            var result = (await repo.GetAllAsync()).FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal("Test EC Sensor", result.Name);
        }
    }
}
