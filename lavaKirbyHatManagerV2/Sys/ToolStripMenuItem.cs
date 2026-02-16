using System;

namespace System.Windows.Forms
{
    public class ToolStripMenuItem
    {
        // so many stubs
        public string Text { get; set; }

        public object ShortcutKeys { get; set; }
        public string ShortcutKeyDisplayString { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<ToolStripMenuItem> DropDownItems
            { get; } = new System.Collections.ObjectModel.ObservableCollection<ToolStripMenuItem>();

        public event EventHandler Click;

        public void PerformClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public string Name { get; set; }
        public object Size { get; set; }
    }
}
