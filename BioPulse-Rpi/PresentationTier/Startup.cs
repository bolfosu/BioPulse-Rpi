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
using Microsoft.Extensions.Configuration;


public class Startup

{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {   
        // Retrieve the database path from configurationf
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
