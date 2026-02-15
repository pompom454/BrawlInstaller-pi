using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BrawlInstaller.StaticClasses;
using System.IO;

namespace BrawlInstaller
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // hookie :D
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Exit += OnExit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            if (Directory.Exists(Paths.TempPath))
            {
                Directory.Delete(Paths.TempPath, true);
            }
        }
    }
}
