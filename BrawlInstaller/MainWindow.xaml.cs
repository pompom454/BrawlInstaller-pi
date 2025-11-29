using Avalonia.Controls;
using BrawlInstaller.Common;
using BrawlInstaller.ViewModels;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel.Composition;

namespace BrawlInstaller
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CompositionBootstrapper.InitializeContainer(this);
            InitializeComponent();
            DataContext = this;
        }

        [Import]
        public IMainViewModel MainViewModel { get; set; }
    }
}