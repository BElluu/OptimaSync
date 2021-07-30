using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using Serilog;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OptimaSync.Service
{
    internal class RegisterDLLService
    {
        SyncUI syncUI = new SyncUI();
        BuildSyncServiceHelper buildSyncHelper = new BuildSyncServiceHelper();
        public void RegisterOptima(string path, bool isProgrammer)
        {
            string registerFile;

            if (path == null)
            {
                return;
            }

            if (isProgrammer)
            {
                path = Properties.Settings.Default.ProgrammersPath;
                registerFile = "RejestrProgramisty.bat";
            }
            else
            {
                registerFile = "Rejestr.bat";
            }

            Process proc;
            try
            {
                syncUI.ChangeProgressLabel(Messages.REGISTER_OPTIMA_INPROGRESS);
                proc = new Process();
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
