﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataAccessLayer;
using Avalonia;
using Avalonia.ReactiveUI;
using System.Linq;

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

                // Get the database path relative to the DataAccessLayer directory
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\DataAccessLayer\hydroponicsystem.db");
                var fullPath = Path.GetFullPath(dbPath);

                // Apply migrations
                using (var context = new AppDbContext(
                    new DbContextOptionsBuilder<AppDbContext>()
                        .UseSqlite($"Data Source={fullPath}")
                        .Options))
                {
                    Console.WriteLine("Applying migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }

                // Test database connection
                using (var context = new AppDbContext(
                    new DbContextOptionsBuilder<AppDbContext>()
                        .UseSqlite($"Data Source={fullPath}")
                        .Options))
                {
                    try
                    {
                        var userCount = context.Users.Count();
                        Console.WriteLine($"Users table exists. User count: {userCount}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error testing database: {ex.Message}");
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
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services);
                });

        public static AppBuilder BuildAvaloniaApp(IHost host) =>
            AppBuilder.Configure(() =>
            {
                var app = new App { ServiceProvider = host.Services };
                return app;
            })
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }
}