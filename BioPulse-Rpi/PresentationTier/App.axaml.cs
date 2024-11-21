using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;
using Avalonia.ReactiveUI;

namespace PresentationTier
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var host = BuildHost();
                ServiceProvider = host.Services;

                // Set up the main window
                desktop.MainWindow = new MainWindow
                {
                    DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services); // Add your DI setup here
                })
                .Build();
        }

        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()   // Automatically detect and use the platform (Windows, Linux, Mac)
                .LogToTrace()           // Optionally log Avalonia trace messages
                .UseReactiveUI();       // If you're using ReactiveUI
    }
}
