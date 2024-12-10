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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Startup
{


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "BioPulse API",
                Version = "v1",
                Description = "API for managing BioPulse features"
            });
        });
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin() // Allow all origins
                    .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
                    .AllowAnyHeader(); // Allow all headers
            });
        });
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
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // Enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BioPulse API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        // Enable CORS
        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }


}
