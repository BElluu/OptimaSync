using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Serilog.Events;

namespace OptimaSync.Service
{
    public class RegisterOptimaService
    {
        protected RegisterOptimaService()
        {
        }
        public static void RegisterOptima(string path)
        {
            string registerFile = "Rejestr.bat";

            if (path == null)
            {
                return;
            }

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.PROGRAMMER.ToString())
            {
                path = AppConfigHelper.GetConfigValue("ProgrammerDestination");
                registerFile = "RejestrProgramisty.bat";
            }

            RegisterDLLFile(registerFile, path);
            RunOptimaService.Start(path);
        }

        private static void RegisterDLLFile(string registerFile, string path)
        {
            try
            {
                SyncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_INPROGRESS);
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.FileName = registerFile;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                proc.WaitForExit();
                Logger.Write(LogEventLevel.Information, Messages.OPTIMA_REGISTERED);
                DownloadServiceHelper.DeleteLockFile(path);
                SyncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_SUCCESSFUL);
                SyncUI.Invoke(() => MainForm.Notification(Messages.REGISTER_OPTIMA_SUCCESSFUL, NotificationForm.notificationType.Success));

            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                SyncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                MessageBox.Show(Messages.REGISTER_OPTIMA_ERROR, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
