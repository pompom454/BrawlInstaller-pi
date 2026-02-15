using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace BrawlInstaller.Behaviors
{
    public static class ContextMenuLeftClickBehavior
    {
        public static readonly AttachedProperty<bool>
            IsLeftClickEnabledProperty =
            AvaloniaProperty.RegisterAttached<
                Control,
                bool>(
                "IsLeftClickEnabled",
                typeof(ContextMenuLeftClickBehavior),
                false);

        public static bool GetIsLeftClickEnabled(Control element)
            => element.GetValue(IsLeftClickEnabledProperty);

        public static void SetIsLeftClickEnabled(
            Control element,
            bool value)
        {
            element.SetValue(
                IsLeftClickEnabledProperty,
                value);

            if (value)
                element.PointerReleased += OnPointerReleased;
            else
                element.PointerReleased -= OnPointerReleased;
        }

        private static void OnPointerReleased(
            object sender,
            PointerReleasedEventArgs e)
        {
            if (sender is Control control)
            {
                if (control.ContextMenu != null)
                {
                    if (control.ContextMenu.DataContext == null)
                    {
                        control.ContextMenu.DataContext =
                            control.DataContext;
                    }

                    control.ContextMenu.Open(control);
                }
            }
        }
    }
}
