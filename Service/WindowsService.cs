using Serilog;
using System;
using System.ServiceProcess;

namespace OptimaSync.Service
{
    class WindowsService
    {
        private static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
        public void StopSOAService()
        {
            ServiceController SoaService = new ServiceController(SOA_SERVICE);

            if (SoaService.Status.Equals(ServiceControllerStatus.Running) ||
                SoaService.Status.Equals(ServiceControllerStatus.StartPending))
            {
                try
                {
                  SoaService.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
        }
    }
}
