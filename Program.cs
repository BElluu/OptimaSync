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
            DownloadServiceHelper downloadHelper = new DownloadServiceHelper(validatorUI, windowsService, syncUI);
            SearchBuildServiceHelper searchBuildHelper = new SearchBuildServiceHelper();
            RunOptimaService runOptimaService = new RunOptimaService(syncUI);
            RegisterOptimaService registerDLL = new RegisterOptimaService(syncUI, downloadHelper, runOptimaService);
            SearchOptimaBuildService searchOptimaBuild = new SearchOptimaBuildService(syncUI, searchBuildHelper);
            SearchEDeclarationBuildService searchEDeclarationBuild = new SearchEDeclarationBuildService(syncUI);
            DownloadEDeclarationService downloadEDeclaration = new DownloadEDeclarationService(searchEDeclarationBuild,downloadHelper,syncUI);
            DownloadOptimaService downloadOptima = new DownloadOptimaService(syncUI, registerDLL, downloadHelper,searchOptimaBuild,downloadEDeclaration);
            var controller = new ApplicationController(new MainForm(downloadOptima, syncUI, searchOptimaBuild));
            controller.Run(Environment.GetCommandLineArgs());
        }
    }
}
