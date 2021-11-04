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
                    Process.Start(path + "\\" + "Comarch OPT!MA.exe");
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
