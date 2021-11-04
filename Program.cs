using System.Configuration;
using System.Collections.Specialized;
using OptimaSync.Helper;
using OptimaSync.Service;
using OptimaSync.UI;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;
using OptimaSync.Config;

namespace OptimaSync
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ConfigAppCreator.Create();
            ConfigureSerilog();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SyncUI syncUI = new SyncUI();
            WindowsService windowsService = new WindowsService(syncUI);
            ValidatorUI validatorUI = new ValidatorUI();
            BuildSyncServiceHelper buildSyncServiceHelper = new BuildSyncServiceHelper(validatorUI, windowsService, syncUI);
            RunOptimaService runOptimaService = new RunOptimaService(syncUI);
            RegisterOptimaService registerDLL = new RegisterOptimaService(syncUI, buildSyncServiceHelper, runOptimaService);
            SearchBuildService searchBuildService = new SearchBuildService();
            BuildSyncService buildSyncService = new BuildSyncService(syncUI, registerDLL, buildSyncServiceHelper,searchBuildService);
            var controller = new ApplicationController(new MainForm(buildSyncService, syncUI, searchBuildService));
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
