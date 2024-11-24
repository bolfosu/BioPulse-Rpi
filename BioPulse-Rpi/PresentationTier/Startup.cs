using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=hydroponicsystem.db"));

        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<UserRepo>();

        // Register services
        services.AddTransient<UserManagementService>();

        // Register ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<MainViewModel>();
    }
}
