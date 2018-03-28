using Serilog;
using System.Windows;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                    .WriteTo.File("Configurator.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                    .CreateLogger();
        }
    }
}
