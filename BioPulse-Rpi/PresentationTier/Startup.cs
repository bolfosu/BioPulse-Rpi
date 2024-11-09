using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Repositories;


namespace PresentationTier
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register DbContext with SQLite connection
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=hydroponicsystem.db"));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IRepository<TemperatureSensor>, TemperatureSensorRepo>();

            // Register any other services or ViewModels here
            // services.AddTransient<MainViewModel>();
        }
    }
}
