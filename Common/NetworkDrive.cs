using OptimaSync.Constant;
using OptimaSync.UI;
using Serilog.Events;
using System;
using System.Net.NetworkInformation;

namespace OptimaSync.Common
{
    public static class NetworkDrive
    {
        public static bool HaveAccessToHost(string HostName)
        {
            Ping pinger = new Ping();

            try
            {
                PingReply pingReply = pinger.Send(HostName, 5000);
                if(pingReply.Status == IPStatus.Success)
                {
                    return true;
                }else
                {
                    SyncUI.Invoke(() => MainForm.Notification("Brak dostępu do " + HostName, NotificationForm.notificationType.Error));
                    Logger.Write(LogEventLevel.Error, "Brak dostępu do " + HostName + "! Sprawdź czy masz internet lub połączenie VPN.");
                }
                return false;
            }
            catch (Exception ex)
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.notificationType.Error));
                Logger.Write(LogEventLevel.Error, ex.Message);
                return false;
            }
            finally
            {
                pinger.Dispose();
            }
        }
    }
}
