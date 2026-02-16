using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BrawlInstaller.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            // Hook SelectionChanged to handle auto-select first enabled tab if needed
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
        }

        private void MainTabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            // Ensure the selected tab is enabled and not "Files"
            if (MainTabControl.SelectedItem is TabItem selectedItem)
            {
                if (!selectedItem.IsEnabled && selectedItem.Name != "filesTab")
                {
                    var firstEnabled = MainTabControl.Items
                        .OfType<TabItem>()
                        .FirstOrDefault(x => x.IsEnabled && x.Name != "filesTab");

                    if (firstEnabled != null)
                        MainTabControl.SelectedItem = firstEnabled;
                }
            }
        }
    }
}