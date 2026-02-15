using Avalonia;
using Avalonia.ReactiveUI;
using Velopack;
using System;

namespace BrawlInstaller
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // more hookies! supa fun i do say so myself
            VelopackApp.Build().Run();

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI();
    }
}
