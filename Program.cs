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
            WindowsService windowsService = new WindowsService();
            DownloadServiceHelper downloadHelper = new DownloadServiceHelper(windowsService);
            RunOptimaService runOptimaService = new RunOptimaService(syncUI);
            RegisterOptimaService registerDLL = new RegisterOptimaService(runOptimaService);
            SearchOptimaBuildService searchOptimaBuild = new SearchOptimaBuildService();
            SearchEDeclarationBuildService searchEDeclarationBuild = new SearchEDeclarationBuildService();
            DownloadEDeclarationService downloadEDeclaration = new DownloadEDeclarationService(searchEDeclarationBuild,syncUI);
            DownloadOptimaService downloadOptima = new DownloadOptimaService(registerDLL, downloadHelper,searchOptimaBuild,downloadEDeclaration);
            var controller = new ApplicationController(new MainForm(downloadOptima, syncUI, searchOptimaBuild));
            controller.Run(Environment.GetCommandLineArgs());
        }
    }
}
