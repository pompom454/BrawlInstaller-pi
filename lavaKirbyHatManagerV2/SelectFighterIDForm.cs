using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace lKHM
{
    public partial class SelectFighterIDForm : Window
    {
        private KirbyHatManager sourceManager = null;

        private bool hatSlotIsPopulated(uint FID)
        {
            return sourceManager.fighterIDToInfoPacks.ContainsKey(FID);
        }

        private void updateSlotNameText()
        {
            uint selectedHatID = (uint)numericUpDownFID.Value;
            if (hatSlotIsPopulated(selectedHatID))
            {
                textBoxSlotName.Text = HatNames.getNameFromFID(selectedHatID);
            }
            else
            {
                textBoxSlotName.Text = "EMPTY_SLOT";
            }
        }

        public SelectFighterIDForm(string contextStringIn, KirbyHatManager managerIn)
        {
            sourceManager = managerIn;
            labelContext.Text = contextStringIn;

            var numTextBox = numericUpDownFID.FindControl<TextBox>("PART_TextBox");
            if (numTextBox != null)
            {
                numTextBox.CharacterCasing = CharacterCasing.Upper;
                numTextBox.MaxLength = 2;
            }

            updateSlotNameText();
        }

        private void buttonOkay_Click(object? sender, RoutedEventArgs e)
        {
            if (hatSlotIsPopulated((uint)numericUpDownFID.Value))
            {
                var dialog = new Window
                {
                    Width = 400,
                    Height = 150,
                    Title = "Confirm Overwrite",
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "Destination Hat Slot is already configured! Overwrite the associated configuration?",
                                Margin = new Avalonia.Thickness(10)
                            },
                            new StackPanel
                            {
                                Orientation = Avalonia.Layout.Orientation.Horizontal,
                                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                                Children =
                                {
                                    new Button
                                    {
                                        Content = "OK",
                                        Margin = new Avalonia.Thickness(10),
                                        Command = ReactiveCommand.Create(() => dialog.Close(true))
                                    },
                                    new Button
                                    {
                                        Content = "Cancel",
                                        Margin = new Avalonia.Thickness(10),
                                        Command = ReactiveCommand.Create(() => dialog.Close(false))
                                    }
                                }
                            }
                        }
                    }
                };
                dialog.ShowDialog(this);
            }

            Close();
        }

        private void buttonCancel_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void numericUpDownFID_KeyDown(object? sender, KeyEventArgs e)
        {
            string allowedChars = "0123456789ABCDEF";
            var keyChar = e.Key.ToString().ToUpper();

            if (!allowedChars.Contains(keyChar) &&
                e.Key != Key.Back &&
                e.Key != Key.Delete &&
                e.Key != Key.Left &&
                e.Key != Key.Right &&
                e.Key != Key.Up &&
                e.Key != Key.Down &&
                e.Key != Key.Shift)
            {
                e.Handled = true;
            }
        }

        private void numericUpDownFID_ValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            updateSlotNameText();
        }
    }
}
