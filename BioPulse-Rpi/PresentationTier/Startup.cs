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

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Get the database path relative to the DataAccessLayer directory
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\DataAccessLayer\hydroponicsystem.db");
        var fullPath = Path.GetFullPath(dbPath);

        // Log the database path
        Console.WriteLine($"[Startup] Database file path: {fullPath}");

        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={fullPath}"));

        // Register views
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<PlantProfileViewModel>();
        services.AddSingleton<DeviceSettingsViewModel>();
        services.AddSingleton<UserSettingsViewModel>();
        services.AddSingleton<PlantProfileViewModel>();
        services.AddTransient<PlantProfileView>();


        // Register New Profile components
        services.AddTransient<NewPlantProfileView>();
        services.AddTransient<NewPlantProfileViewModel>();

        // Register repositories
        services.AddScoped<UserRepo>();
        services.AddScoped<IRepository<PlantProfile>, PlantProfileRepo>();
        services.AddScoped<IRepository<Sensor>, GenericRepository<Sensor>>();

        // Register services
        services.AddSingleton<UserManagementService>();
        services.AddSingleton<PlantProfileService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordRecoveryViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<PlantProfileViewModel>();
        services.AddSingleton<DeviceSettingsViewModel>();
        services.AddSingleton<UserSettingsViewModel>();
        services.AddTransient<NewPlantProfileViewModel>(); // Registering NewProfileViewModel as transient for dynamic navigation
    }
}
