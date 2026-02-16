using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Input;
using Avalonia.Media;

namespace BrawlLib.BrawlManagerLib.Songs
{
    partial class SongNameBar
    {
        private Button button1;
        private MyTextBox textBox1;
        private Panel panel1;
        private Button button2;

        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new MyTextBox();
            panel1 = new StackPanel();
            button2 = new Button();

            Width = 250;
            Height = 20;

            var root = new DockPanel();

            panel1.Orientation = Orientation.Horizontal;
            panel1.Width = 110;
            DockPanel.SetDock(panel1, Dock.Right);

            button1.Content = "Restore";
            button1.Width = 55;
            button1.Height = 20;
            button1.Click += button1_Click;

            button2.Content = "Default";
            button2.Width = 55;
            button2.Height = 20;
            button2.Click += button2_Click;

            panel1.Children.Add(button1);
            panel1.Children.Add(button2);

            textBox1.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBox1.VerticalAlignment = VerticalAlignment.Stretch;
            textBox1.Height = 20;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyUp += textBox1_KeyUp;

            DockPanel.SetDock(textBox1, Dock.Left);

            root.Children.Add(panel1);
            root.Children.Add(textBox1);

            Content = root;
        }

        private class MyTextBox : TextBox
        {
            protected override void OnKeyUp(KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                    e.Handled = true;

                base.OnKeyUp(e);
            }
        }
    }
}