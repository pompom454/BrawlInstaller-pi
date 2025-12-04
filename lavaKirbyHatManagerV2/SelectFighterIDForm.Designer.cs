using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace lKHM
{
    public partial class SelectFighterIDForm : Window
    {
        public NumericUpDown numericUpDownFID;
        public CheckBox checkBoxSetName;
        private TextBlock labelContext;
        private Button buttonOkay;
        private Button buttonCancel;
        private TextBox textBoxSlotName;

        public void InitializeComponent()
        {
            Width = 297;
            Height = 105;
            CanResize = false;
            Title = "Select Fighter ID";

            var grid = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,Auto,Auto"),
                ColumnDefinitions = new ColumnDefinitions("Auto,*")
            };

            labelContext = new TextBlock
            {
                Margin = new Thickness(13, 13, 0, 5),
                Text = "label1"
            };
            Grid.SetColumnSpan(labelContext, 2);
            grid.Children.Add(labelContext);

            numericUpDownFID = new NumericUpDown
            {
                Margin = new Thickness(16, 0, 10, 0),
                Minimum = 0,
                Maximum = 255,
                FormatString = "X2",
                Width = 58
            };
            Grid.SetRow(numericUpDownFID, 1);
            grid.Children.Add(numericUpDownFID);

            textBoxSlotName = new TextBox
            {
                Margin = new Thickness(0, 0, 10, 0),
                IsReadOnly = true,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Grid.SetRow(textBoxSlotName, 1);
            Grid.SetColumn(textBoxSlotName, 1);
            grid.Children.Add(textBoxSlotName);

            checkBoxSetName = new CheckBox
            {
                Margin = new Thickness(16, 10, 0, 0),
                Content = "Copy Name"
            };
            Grid.SetRow(checkBoxSetName, 2);
            grid.Children.Add(checkBoxSetName);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 10, 0)
            };

            buttonOkay = new Button
            {
                Content = "Okay",
                Width = 80,
                Margin = new Thickness(0, 0, 10, 0)
            };
            buttonOkay.Click += buttonOkay_Click;

            buttonCancel = new Button
            {
                Content = "Cancel",
                Width = 80
            };
            buttonCancel.Click += buttonCancel_Click;

            btnPanel.Children.Add(buttonOkay);
            btnPanel.Children.Add(buttonCancel);
            Grid.SetRow(btnPanel, 2);
            Grid.SetColumn(btnPanel, 1);
            grid.Children.Add(btnPanel);

            Content = grid;

            numericUpDownFID.ValueChanged += numericUpDownFID_ValueChanged;
            numericUpDownFID.KeyDown += numericUpDownFID_KeyDown;
        }
    }
}
}
