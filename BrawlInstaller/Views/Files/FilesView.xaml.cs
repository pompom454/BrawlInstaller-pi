using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BrawlInstaller.Views
{
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}