using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class TreeNode : INotifyPropertyChanged
    {
        private string _text;

        public TreeNode()
        {
            Nodes = new ObservableCollection<TreeNode>();
        }

        public ObservableCollection<TreeNode> Nodes { get; }

        public TreeNode Parent { get; internal set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public object Tag { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
