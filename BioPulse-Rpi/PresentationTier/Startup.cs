using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;
using System.IO;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;

public class Startup

{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {// Retrieve the database path from configuration
        string dbPath = _configuration["DatabaseSettings:DatabasePath"];
        if (string.IsNullOrEmpty(dbPath))
        {
            // Fallback or handle missing path scenario
            dbPath = "hydroponicsystem.db";
        }

        // Register DbContext using the retrieved path
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        // Register views
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<PlantProfileViewModel>();
        services.AddSingleton<DeviceSettingsViewModel>();
        services.AddSingleton<UserSettingsViewModel>();
        services.AddSingleton<PlantProfileViewModel>();

        services.AddSingleton<PlantProfileView>();


     

        // Register repositories
        services.AddScoped<UserRepo>();
        services.AddScoped<IRepository<PlantProfile>, GenericRepository<PlantProfile>>();
        services.AddScoped<IRepository<Sensor>, GenericRepository<Sensor>>();

        // Register services
        services.AddSingleton<UserManagementService>();
        services.AddSingleton<PlantProfileService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordRecoveryViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<DeviceSettingsViewModel>();
        services.AddSingleton<UserSettingsViewModel>();
       
    }
}
