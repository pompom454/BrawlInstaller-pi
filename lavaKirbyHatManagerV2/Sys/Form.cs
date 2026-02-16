using Avalonia.Controls;

namespace System.Windows.Forms
{
    public class Form : Window
    {
        public object AcceptButton { get; set; }
        public object CancelButton { get; set; }

        public bool ShowIcon { get; set; }
        public bool ShowInTaskbar { get; set; }

        public FormBorderStyle FormBorderStyle { get; set; }

        public FormStartPosition StartPosition { get; set; }

        public DialogResult DialogResult { get; set; }

        public void Close()
        {
            base.Close();
        }

        public DialogResult ShowDialog()
        {
            base.ShowDialog();
            return DialogResult;
        }
    }

    public enum FormBorderStyle
    {
        None,
        FixedSingle,
        FixedDialog,
        FixedToolWindow
    }

    public enum FormStartPosition
    {
        CenterParent,
        CenterScreen
    }

    public enum DialogResult
    {
        None,
        OK,
        Cancel
    }
}
