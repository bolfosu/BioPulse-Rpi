using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.Extensions.Logging;
using LogicLayer.Services;

namespace PresentationTier
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Start API synchronously
                StartApiHost(args);

                // Start Avalonia application
                var host = CreateHostBuilder(args).Build();
                StartBackgroundServices(host);
                var appBuilder = BuildAvaloniaApp(host);

                if (args.Contains("--drm"))
                {
                    SilenceConsole();
                    appBuilder.StartLinuxDrm(args: args, card: null, scaling: 1.0);
                }
                else
                {
                    appBuilder.StartWithClassicDesktopLifetime(args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application startup failed: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void StartApiHost(string[] args)
        {
            try
            {
                Console.WriteLine("Starting API host...");
                var apiHost = Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://0.0.0.0:5000"); // Bind to all network interfaces
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                    })
                    .Build();

                Console.WriteLine("API host running...");
                apiHost.RunAsync(); // Run API host in the background
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API host failed to start: {ex.Message}");
                throw;
            }
        }

        private static void StartBackgroundServices(IHost host)
        {
            try
            {
                using var scope = host.Services.CreateScope();
                var i2cService = scope.ServiceProvider.GetRequiredService<I2cReadingService>();
                Console.WriteLine("I2C reading service initialized and running.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting I2C service: {ex.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenAnyIP(5000); // Bind to all network interfaces (HTTP)
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    var startup = new Startup(configuration);
                    startup.ConfigureServices(services);
                });

        public static AppBuilder BuildAvaloniaApp(IHost host) =>
            AppBuilder.Configure<App>()
                      .UsePlatformDetect()
                      .LogToTrace()
                      .UseReactiveUI()
                      .AfterSetup(_ => { ((App)Application.Current).ServiceProvider = host.Services; });

        private static void SilenceConsole()
        {
            new System.Threading.Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
            { IsBackground = true }.Start();
        }
    }
}