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


        

        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=hydroponicsystem.db"));

        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();

        // Register repositories
        services.AddScoped<UserRepo>();

        // Register services
        services.AddSingleton<UserManagementService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();

       
       

    }
}
