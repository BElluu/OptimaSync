using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace OptimaSync
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ConfigureSerilog();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var controller = new ApplicationController(new MainForm());
            controller.Run(Environment.GetCommandLineArgs());
        }

        private static void ConfigureSerilog()
        {
            var logFile = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "OSync\\OSync.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 2097152, retainedFileCountLimit: 7)
                .CreateLogger();
        }
    }
}
