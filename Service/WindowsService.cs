using Serilog;
using System;
using System.Linq;
using System.ServiceProcess;

namespace OptimaSync.Service
{
    class WindowsService
    {
        private static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
        ServiceController SoaService = new ServiceController(SOA_SERVICE);
        public void StopSOAService()
        {
            if (DoesSOAServiceExist())
            {
                if (SoaService.Status.Equals(ServiceControllerStatus.Running) ||
                    SoaService.Status.Equals(ServiceControllerStatus.StartPending))
                {
                    try
                    {
                        SoaService.Stop();
                        Log.Information("Zatrzymano " + SOA_SERVICE);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                    }
                }
            }
            else
            {
                Log.Error("Na stanowisku nie ma " + SOA_SERVICE + ". Zarejestrowanie Optimy z usługą SOA nie jest możliwe");
            }
        }

        private bool DoesSOAServiceExist()
        {
            ServiceController windowsServices = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == SOA_SERVICE);
            if (windowsServices == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
