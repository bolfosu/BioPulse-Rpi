using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PresentationTier.ViewModels;

namespace PresentationTier.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}