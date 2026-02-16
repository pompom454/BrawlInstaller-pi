using Avalonia.Controls;

namespace System.Windows.Forms
{
    public class CheckBox : Avalonia.Controls.CheckBox
    {
        public bool Checked
        {
            get => IsChecked ?? false;
            set => IsChecked = value;
        }
    }
}
