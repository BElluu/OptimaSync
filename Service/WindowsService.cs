using Serilog;
using System;
using System.Linq;
using System.ServiceProcess;

namespace OptimaSync.Service
{
    class WindowsService
    {
        public static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
        ServiceController SoaService = new ServiceController(SOA_SERVICE);
        public int StopSOAService()
        {
            if (SoaService.Status.Equals(ServiceControllerStatus.Running) ||
                SoaService.Status.Equals(ServiceControllerStatus.StartPending))
            {
                try
                {
                    SoaService.Stop();
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
                Log.Error("Usługa SOA jest zatrzymana");
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
