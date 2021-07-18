using OptimaSync.Constant;
using OptimaSync.UI;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace OptimaSync.Service
{
    internal class WindowsService
    {
        SyncUI syncUI = new SyncUI();
        public static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
        public int StopSOAService()
        {
            ServiceController SoaService = new ServiceController(SOA_SERVICE);
            if (SoaService.Status.Equals(ServiceControllerStatus.Running) ||
                SoaService.Status.Equals(ServiceControllerStatus.StartPending) ||
                SoaService.Status.Equals(ServiceControllerStatus.ContinuePending))
            {
                try
                {
                    syncUI.ChangeProgressLabel(Messages.STOPPING_SOA_SERVICE);
                    SoaService.Stop();
                    SoaService.WaitForStatus(ServiceControllerStatus.Stopped);

                    foreach (var process in Process.GetProcessesByName("ComarchOptimaSerwisOperacjiAutomatycznych"))
                    {
                        process.Kill();
                    }
                    Log.Information("Zatrzymano " + SOA_SERVICE);
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    return 1;
                }
            }

            else if (SoaService.Status.Equals(ServiceControllerStatus.Stopped))
            {
                Log.Information("Usługa SOA jest zatrzymana");
                return 0;
            }
            else if (SoaService.Status.Equals(ServiceControllerStatus.StopPending))
            {
                SoaService.WaitForStatus(ServiceControllerStatus.Stopped);
                Log.Information("Usługa SOA jest zatrzymana");
                return 0;
            }
            else
            {
                Log.Error("Stan usługi SOA jest nieznany!");
                return 1;
            }
        }

        public bool DoesSOAServiceExist()
        {
            ServiceController windowsServices = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == SOA_SERVICE);
            if (windowsServices == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
