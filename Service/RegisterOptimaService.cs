using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OptimaSync.Service
{
    public class RegisterOptimaService
    {
        SyncUI syncUI;
        BuildSyncServiceHelper buildSyncHelper;
        RunOptimaService runOptima;

        public RegisterOptimaService(SyncUI syncUI, BuildSyncServiceHelper buildSyncHelper, RunOptimaService runOptima)
        {
            this.syncUI = syncUI;
            this.buildSyncHelper = buildSyncHelper;
            this.runOptima = runOptima;
        }
        public void RegisterOptima(string path)
        {
            string registerFile = "Rejestr.bat";

            if (path == null)
            {
                return;
            }

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.PROGRAMMER.ToString())
            {
                path = AppConfigHelper.GetConfigValue("ProgrammerDestination");
                registerFile = "RejestrProgramisty.bat";
            }

            RegisterDLLFile(registerFile, path);
            runOptima.Start(path);
        }

        private void RegisterDLLFile(string registerFile, string path)
        {
            try
            {
                syncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_INPROGRESS);
                Process proc = new Process();
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.FileName = registerFile;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                proc.WaitForExit();
                Log.Information(Messages.OPTIMA_REGISTERED);
                buildSyncHelper.DeleteLockFile(path);
                syncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_SUCCESSFUL);
                SyncUI.Invoke(() => MainForm.Notification(Messages.REGISTER_OPTIMA_SUCCESSFUL, NotificationForm.enumType.Success));

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                MessageBox.Show(Messages.REGISTER_OPTIMA_ERROR, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
