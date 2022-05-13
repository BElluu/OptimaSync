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
            DownloadServiceHelper downloadHelper = new DownloadServiceHelper();
            RegisterOptimaService registerDLL = new RegisterOptimaService();
            SearchOptimaBuildService searchOptimaBuild = new SearchOptimaBuildService();
            DownloadEDeclarationService downloadEDeclaration = new DownloadEDeclarationService();
            DownloadOptimaService downloadOptima = new DownloadOptimaService(registerDLL, downloadHelper,downloadEDeclaration);
            var controller = new ApplicationController(new MainForm(downloadOptima, searchOptimaBuild));
            controller.Run(Environment.GetCommandLineArgs());
        }
    }
}
