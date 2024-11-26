using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccessLayer;
using Avalonia;
using System.Linq;
using Avalonia.ReactiveUI;

namespace PresentationTier
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting application...");

                // Test SQLite connection
                using (var context = new AppDbContext(
                    new DbContextOptionsBuilder<AppDbContext>()
                        .UseSqlite("Data Source=hydroponicsystem.db")
                        .Options))
                {
                    try
                    {
                        Console.WriteLine("Testing database connection...");

                        // Log the full path of the database
                        var dbPath = "hydroponicsystem.db";
                        var fullPath = Path.GetFullPath(dbPath);
                        Console.WriteLine($"Database file path: {fullPath}");

                        // Test Users table existence
                        var userCount = context.Users.Count();
                        Console.WriteLine($"Users table exists. User count: {userCount}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accessing Users table: {ex.Message}");
                    }
                }

                Console.WriteLine("Application initialized successfully.");

                // Start the Avalonia app
                var host = CreateHostBuilder(args).Build();
                BuildAvaloniaApp(host).StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error during startup: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    try
                    {
                        var startup = new Startup();
                        startup.ConfigureServices(services);
                        Console.WriteLine("Services configured successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during service configuration: {ex.Message}");
                        throw;
                    }
                });

        public static AppBuilder BuildAvaloniaApp(IHost host)
        {
            Console.WriteLine("Building Avalonia App...");
            return AppBuilder.Configure(() =>
            {
                var app = new App
                {
                    ServiceProvider = host.Services
                };
                return app;
            })
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
        }
    }
}
