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
            DownloadServiceHelper downloadHelper = new DownloadServiceHelper();
            DownloadOptimaService downloadOptima = new DownloadOptimaService(downloadHelper);
            var controller = new ApplicationController(new MainForm(downloadOptima));
            controller.Run(Environment.GetCommandLineArgs());
        }
    }
}
