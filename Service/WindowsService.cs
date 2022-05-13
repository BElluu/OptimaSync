using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using Serilog.Events;

namespace OptimaSync.Service
{
    public class WindowsService
    {
        public static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
        public static readonly string SOA_PROCESS = "ComarchOptimaSerwisOperacjiAutomatycznych";

        public WindowsService()
        {
        }

        public bool SoaIsStopped()
        {
            return StopSOAService();
        }

        public static bool DoesSOAServiceExist()
        {
            ServiceController windowsServices = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == SOA_SERVICE);
            if (windowsServices == null)
            {
                return false;
            }
            return true;
        }

        private bool StopSOAService()
        {
            ServiceController SoaService = new ServiceController(SOA_SERVICE);
            if (SoaService.Status.Equals(ServiceControllerStatus.Running) ||
                SoaService.Status.Equals(ServiceControllerStatus.StartPending) ||
                SoaService.Status.Equals(ServiceControllerStatus.ContinuePending))
            {
                try
                {
                    SyncUI.ChangeProgressLabel(Messages.STOPPING_SOA_SERVICE);
                    SoaService.Stop();
                    SoaService.WaitForStatus(ServiceControllerStatus.Stopped);

                    foreach (var process in Process.GetProcessesByName(SOA_PROCESS))
                    {
                        process.Kill();
                    }
                    Logger.Write(LogEventLevel.Information, "Zatrzymano " + SOA_SERVICE);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Write(LogEventLevel.Error, ex.Message);
                    return false;
                }
            }

            else if (SoaService.Status.Equals(ServiceControllerStatus.Stopped))
            {
                Logger.Write(LogEventLevel.Information, Messages.SOA_SERVICE_IS_STOPPED);
                return true;
            }
            else if (SoaService.Status.Equals(ServiceControllerStatus.StopPending))
            {
                SoaService.WaitForStatus(ServiceControllerStatus.Stopped);
                Logger.Write(LogEventLevel.Information, Messages.SOA_SERVICE_IS_STOPPED);
                return true;
            }
            else
            {
                Logger.Write(LogEventLevel.Error, Messages.SOA_SERVICE_UNKNOWN_STATUS);
                return false;
            }
        }
    }
}
