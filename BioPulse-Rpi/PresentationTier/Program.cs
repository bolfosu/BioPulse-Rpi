using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
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
                Console.WriteLine("Starting Avalonia application...");

                var host = CreateHostBuilder(args).Build();
                var appBuilder = BuildAvaloniaApp(host);

                if (args.Contains("--drm")) // If running on Raspberry Pi framebuffer
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
                Console.WriteLine($"Application failed to start: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Add appsettings.json and environment variables
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Retrieve configuration from hostContext
                    var configuration = hostContext.Configuration;

                    // Pass configuration into Startup
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
            new Thread(() =>
            {
                Console.CursorVisible = false;
                while (true)
                    Console.ReadKey(true);
            })
            { IsBackground = true }.Start();
        }
    }
}
