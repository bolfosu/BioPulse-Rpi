using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using LogicLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;




public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Retrieve the database path from configuration
        string dbPath = _configuration["DatabaseSettings:DatabasePath"] ?? "hydroponicsystem.db";

        // Register DbContextFactory for thread-safe usage of DbContext
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddControllers();

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
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
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
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
        services.AddSingleton<UserSettingsView>();

        // Register repositories with IDbContextFactory
        services.AddScoped<IRepository<PlantProfile>, GenericRepository<PlantProfile>>();
        services.AddScoped<IRepository<Sensor>, GenericRepository<Sensor>>();
        services.AddScoped<IRepository<SensorReading>, GenericRepository<SensorReading>>();
        services.AddScoped<UserRepo>();

        // Register services
        services.AddTransient<UserManagementService>();
        services.AddTransient<PlantProfileService>();
        services.AddTransient<SensorDataIngestionService>();
        

        // Register I2C Reading Service
        services.AddTransient<I2cReadingService>(sp =>
        {
            var ingestionService = sp.GetRequiredService<SensorDataIngestionService>();
            var logger = sp.GetRequiredService<ILogger<I2cReadingService>>();
            return new I2cReadingService(ingestionService, logger, busId: 1, address: 0x61);
        });

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordRecoveryViewModel>();
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

        // Log registered routes
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            var routeEndpoints = endpoints.DataSources
                .SelectMany(ds => ds.Endpoints)
                .OfType<Microsoft.AspNetCore.Routing.RouteEndpoint>();

            foreach (var route in routeEndpoints)
            {
                Console.WriteLine($"Registered route: {route.RoutePattern.RawText}");
            }
        });
    }
}
