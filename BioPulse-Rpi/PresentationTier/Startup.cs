using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using LogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;
using System.IO;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Get the database path in the publish directory
        var dbPath = Path.Combine(AppContext.BaseDirectory, "hydroponicsystem.db");

        // Log the database path for debugging
        Console.WriteLine($"[Startup] Database file path: {dbPath}");

        // Add DbContext with the resolved database path
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // Views
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainSingleView>();
        services.AddSingleton<MainView>();
        services.AddSingleton<PlantProfileView>();
        services.AddSingleton<DeviceSettingsView>();
        services.AddSingleton<UserSettingsView>();
        services.AddSingleton<RegistrationView>();
        services.AddSingleton<PasswordRecoveryView>();
        services.AddSingleton<DashboardView>();
        services.AddSingleton<LoginView>();
        

        // ViewModels
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<DeviceSettingsViewModel>();
        services.AddTransient<UserSettingsViewModel>();
        services.AddTransient<PlantProfileViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordRecoveryViewModel>();




        services.AddScoped<UserRepo>();
        services.AddScoped<IRepository<PlantProfile>, GenericRepository<PlantProfile>>();
        services.AddScoped<IRepository<Sensor>, GenericRepository<Sensor>>();

        services.AddSingleton<UserManagementService>();
        services.AddSingleton<PlantProfileService>();

        
       

    }
}
