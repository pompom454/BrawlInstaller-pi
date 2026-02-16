using Avalonia.Controls;
using System.Collections.ObjectModel;
using System;

namespace System.Windows.Forms
{
    public class MenuStrip
    {
        public ObservableCollection<ToolStripMenuItem> Items { get; } 
            = new ObservableCollection<ToolStripMenuItem>();

        // stubby chubby
        public object ImageScalingSize { get; set; }
        public object Location { get; set; }
        public object Name { get; set; }
        public object Size { get; set; }
        public object TabIndex { get; set; }
        public object Text { get; set; }

        public void SuspendLayout() { }
        public void ResumeLayout(bool performLayout) { }
        public void PerformLayout() { }
    }
}
