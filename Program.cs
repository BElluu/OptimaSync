using OptimaSync.Helper;
using OptimaSync.Service;
using OptimaSync.UI;
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
            SyncUI syncUI = new SyncUI();
            WindowsService windowsService = new WindowsService(syncUI);
            ValidatorUI validatorUI = new ValidatorUI();
            BuildSyncServiceHelper buildSyncServiceHelper = new BuildSyncServiceHelper(validatorUI, windowsService, syncUI);
            RegisterDLLService registerDLL = new RegisterDLLService(syncUI, buildSyncServiceHelper);
            SearchBuildService searchBuildService = new SearchBuildService();
            BuildSyncService buildSyncService = new BuildSyncService(syncUI, registerDLL, buildSyncServiceHelper,searchBuildService);
            var controller = new ApplicationController(new MainForm(buildSyncService, syncUI, validatorUI, searchBuildService));
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
