using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Hosting;
using System;

namespace PresentationTier
{
    internal class Program
    {
        // Main entry point of the application
        [STAThread]
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Start the Avalonia application
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services); // Configure services
                });

        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()   // Ensure platform detection is enabled
                .LogToTrace()           // Optional logging
                .UseReactiveUI();       // Enable ReactiveUI if used
    }
}