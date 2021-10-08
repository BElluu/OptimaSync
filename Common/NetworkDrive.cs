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
                return pingReply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
            finally
            {
                pinger.Dispose();
            }
        }
    }
}
