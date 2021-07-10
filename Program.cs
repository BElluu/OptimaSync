using OptimaSync.Constant;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace OptimaSync
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigureSerilog();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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
