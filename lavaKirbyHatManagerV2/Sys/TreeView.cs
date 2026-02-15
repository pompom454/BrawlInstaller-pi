using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace System.Windows.Forms
{
    public class TreeView : Avalonia.Controls.TreeView
    {
        public ObservableCollection<TreeNode> Nodes { get; }
            = new ObservableCollection<TreeNode>();

        public TreeView()
        {
            ItemsSource = Nodes;
        }
    }
}
