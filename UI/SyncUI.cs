using OptimaSync.Constant;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class SyncUI
    {
        static readonly string UserManual = "https://osync.devopsowy.pl/Instrukcja.pdf";

        public static void Invoke(Action action)
        {
            MainForm mainForm = Application.OpenForms.Cast<MainForm>().FirstOrDefault();
            if (mainForm != null && mainForm.InvokeRequired)
                mainForm.Invoke(action);
            else
                action();
        }
        public void PathToTextbox(TextBox textBox)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = String.Empty;
            textBox.AppendText(folderBrowserDialog.SelectedPath);
        }

        public void ChangeProgressLabel(string status)
        {
           Invoke(() => MainForm.Instance.ProgressLabelStatus = "Status: " + status);
           Invoke(() => MainForm.Instance.labelProgress.Refresh());
        }

        public string GetAppVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var versionWithoutRevision = version[..8];
            return versionWithoutRevision;
        }

        public void EnableElementsOnForm(bool state)
        {
            MainForm.Instance.downloadBuildButton.Enabled = state;
            MainForm.Instance.SOACheckBox.Enabled = state;
            MainForm.Instance.programmerCheckbox.Enabled = state;
        }

        public void OpenLogsDirectory()
        {

            string logsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OSync");
                if (Directory.Exists(logsDirectory))
                {
                    System.Diagnostics.Process.Start("explorer.exe", logsDirectory);
                }
                else
                {
                    Log.Logger.Error(Messages.LOGS_DIRECTORY_NOT_EXIST);
                    MessageBox.Show(Messages.LOGS_DIRECTORY_NOT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        public void OpenUserManual()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(UserManual) { UseShellExecute = true });
        }

        public void DisableElementsWhileProgrammer(bool isProgrammer)
        {
            bool state;
            if (isProgrammer)
            {
                state = false;
            }
            else
            {
                state = true;
            }

            MainForm.Instance.SOACheckBox.Checked = false;
            MainForm.Instance.SOACheckBox.Enabled = state;
            MainForm.Instance.destDirectoryLabel.Enabled = state;
            MainForm.Instance.DestTextBox.Enabled = state;
            MainForm.Instance.buttonDestinationDirectory.Enabled = state;
            MainForm.Instance.soaDestDirectoryLabel.Enabled = state;
            MainForm.Instance.OptimaSOATextBox.Enabled = state;
            MainForm.Instance.buttonOptimaSOADirectory.Enabled = state;
        }
    }
}
