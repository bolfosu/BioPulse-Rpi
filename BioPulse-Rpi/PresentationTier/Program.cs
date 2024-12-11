using System;
using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;

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
                        webBuilder.UseUrls("http://localhost:5000");
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5000); // Explicitly bind to localhost:5000
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
