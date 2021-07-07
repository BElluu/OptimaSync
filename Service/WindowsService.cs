using OptimaSync.Constants;
using Serilog;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;

namespace OptimaSync.Service
{
    class WindowsService
    {
        public static readonly string SOA_SERVICE = "ComarchAutomatSynchronizacji";
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
                Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.SOA_SERVICE_DONT_EXIST_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool DoesSOAServiceExist()
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
