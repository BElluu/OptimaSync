using OptimaSync.Common;
using OptimaSync.Helper;
using OptimaSync.UI;
using System;
using System.Diagnostics;
using Serilog.Events;

namespace OptimaSync.Service
{
    public class RunOptimaService
    {
        SyncUI syncUI;
        public RunOptimaService(SyncUI syncUI)
        {
            this.syncUI = syncUI;
        }
        public void Start(string path)
        {
            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima")))
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.WorkingDirectory = path;
                    processStartInfo.FileName = path + "\\" + "Comarch OPT!MA.exe";
                    processStartInfo.CreateNoWindow = true;
                    Process.Start(processStartInfo);
                    
                }
                catch (Exception ex)
                {
                    Logger.Write(LogEventLevel.Warning, ex.Message);
                    syncUI.ChangeProgressLabel("Nie udało się uruchomić O!");
                    SyncUI.Invoke(() => MainForm.Notification("Nie udało się uruchomić O!", NotificationForm.enumType.Warning));
                }
            }
        }
    }
}
