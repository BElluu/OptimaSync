using OptimaSync.Helper;
using OptimaSync.Service;
using OptimaSync.UI;
using System;
using System.Windows.Forms;
using OptimaSync.Config;
using OptimaSync.Common;

namespace OptimaSync
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ConfigAppCreator.Create();
            Logger.ConfigureSerilog();
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
            DownloaderService buildSyncService = new DownloaderService(syncUI, registerDLL, buildSyncServiceHelper,searchBuildService);
            var controller = new ApplicationController(new MainForm(buildSyncService, syncUI, searchBuildService));
            controller.Run(Environment.GetCommandLineArgs());
        }
    }
}
