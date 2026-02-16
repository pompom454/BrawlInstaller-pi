using Avalonia.Controls;

namespace System.Windows.Forms
{
    public class NumericUpDown : Avalonia.Controls.NumericUpDown
    {
        public decimal Value
        {
            get => (decimal)(base.Value ?? 0);
            set => base.Value = (double)value;
        }
    }
}
