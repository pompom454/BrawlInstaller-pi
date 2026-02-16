namespace System.Windows.Forms
{
    public class PropertyGrid : Avalonia.Controls.UserControl
    {
        private readonly Bodong.Controls.PropertyGrid grid = new();

        public object SelectedObject
        {
            get => grid.SelectedObject;
            set => grid.DataContext = value;
        }

        public void Refresh() => grid.Refresh();

        public PropertySort PropertySort { get => default; set {} }
    }
}
