using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BrawlInstaller.Views
{
    public partial class MainControlsView : UserControl
    {
        public MainControlsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}