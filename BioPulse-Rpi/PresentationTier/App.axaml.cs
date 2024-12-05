using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PresentationTier.ViewModels;
using PresentationTier.Views;
using System;

namespace PresentationTier
{
    public class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
                desktopLifetime.MainWindow = mainWindow;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                singleViewLifetime.MainView = ServiceProvider.GetRequiredService<MainSingleView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
