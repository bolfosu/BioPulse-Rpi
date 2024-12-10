using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = ServiceProvider.GetRequiredService<MainSingleView>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
