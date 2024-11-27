using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;
using System.IO;
using DataAccessLayer;

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

        // Register repositories
        services.AddScoped<UserRepo>();

        // Register services
        services.AddSingleton<UserManagementService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordRecoveryViewModel>();

    }
}
