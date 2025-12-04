using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace lKHM
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AppBuilder.Configure<Application>()
                          .UsePlatformDetect()
                          .StartWithClassicDesktopLifetime(args, lifetime =>
                          {
                              lifetime.MainWindow = new Window
                              {
                                  Title = "lKHM",
                                  Width = 800,
                                  Height = 600
                              };
                          });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
