using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PresentationTier.Views
{
    public partial class PasswordRecoveryView : UserControl
    {
        public PasswordRecoveryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}