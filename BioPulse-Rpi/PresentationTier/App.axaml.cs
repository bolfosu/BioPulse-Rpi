using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;

namespace PresentationTier
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Ensure ServiceProvider is initialized
                if (ServiceProvider == null)
                {
                    throw new InvalidOperationException("ServiceProvider is not initialized.");
                }

                // Retrieve MainWindow and its ViewModel from ServiceProvider
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                var mainWindowViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();

                // Assign the ViewModel to the MainWindow's DataContext
                mainWindow.DataContext = mainWindowViewModel;

                // Set the MainWindow
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
