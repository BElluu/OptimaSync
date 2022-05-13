using OptimaSync.Common;
using OptimaSync.Helper;
using OptimaSync.UI;
using System;
using System.Diagnostics;
using Serilog.Events;
using System.IO;

namespace OptimaSync.Service
{
    public class RunOptimaService
    {
        public RunOptimaService()
        {
        }
        public static void Start(string path)
        {
            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima")))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.WorkingDirectory = path;
                    processStartInfo.FileName = path + Path.DirectorySeparatorChar + "Comarch OPT!MA.exe";
                    processStartInfo.CreateNoWindow = true;
                    Process.Start(processStartInfo);
                    
                }
                catch (Exception ex)
                {
                    Logger.Write(LogEventLevel.Warning, ex.Message);
                    SyncUI.ChangeProgressLabel("Nie udało się uruchomić O!");
                    SyncUI.Invoke(() => MainForm.Notification("Nie udało się uruchomić O!", NotificationForm.notificationType.Warning));
                }
            }
        }
    }
}
