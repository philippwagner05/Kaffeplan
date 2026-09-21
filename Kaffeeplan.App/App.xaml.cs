using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace Kaffeeplan.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        // Change culture under which this application runs
        CultureInfo ci = new CultureInfo("de-DE");
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        DispatcherUnhandledException += (s, args) =>
        {
            MessageBox.Show($"Unerwarteter Fehler:\n\n{args.Exception.Message}",
                            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        base.OnStartup(e);
    }
}

