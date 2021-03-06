using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Serilog.Events;

namespace OptimaSync.UI
{
    public class SyncUI
    {
        static readonly string USER_MANUAL = "https://osync.devopsowy.pl/Instrukcja.pdf";
        #pragma warning disable S1075
        static readonly string EXPLORER_PATH = @"C:\Windows\explorer.exe";
        #pragma warning restore S1075

        protected SyncUI() { }
        public static void Invoke(Action action)
        {
            MainForm mainForm = Application.OpenForms.Cast<MainForm>().FirstOrDefault();
            if (mainForm != null && mainForm.InvokeRequired)
                mainForm.Invoke(action);
            else
                action();
        }
        public static void PathToTextbox(TextBox textBox)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = String.Empty;
            textBox.AppendText(folderBrowserDialog.SelectedPath);
        }

        public static void ChangeProgressLabel(string status)
        {
            Invoke(() => MainForm.Instance.ProgressLabelStatus = "Status: " + status);
            Invoke(() => MainForm.Instance.labelProgress.Refresh());
        }

        public static string GetAppVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var versionWithoutRevision = version[..8];
            return versionWithoutRevision;
        }

        public static void EnableElementsOnForm(bool state)
        {
            Invoke(() => MainForm.Instance.downloadBuildButton.Enabled = state);
            Invoke(() => MainForm.Instance.SOACheckBox.Enabled = state);
            Invoke(() => MainForm.Instance.programmerCheckbox.Enabled = state);
            Invoke(() => MainForm.Instance.RunOptimaCheckBox.Enabled = state);
            Invoke(() => MainForm.Instance.DestTextBox.Enabled = state);
            Invoke(() => MainForm.Instance.OptimaSOATextBox.Enabled = state);
            Invoke(() => MainForm.Instance.buttonDestinationDirectory.Enabled = state);
            Invoke(() => MainForm.Instance.buttonOptimaSOADirectory.Enabled = state);
            Invoke(() => MainForm.Instance.buildRadio.Enabled = state);
            Invoke(() => MainForm.Instance.prodRadio.Enabled = state);
            Invoke(() => MainForm.Instance.eDeclarationCheckBox.Enabled = state);

            var prodRadioStatus = MainForm.Instance.prodRadio.Checked;
            if(prodRadioStatus)
            {
                Invoke(() => MainForm.Instance.prodVersionDropMenu.Enabled = state);
            }

            if(AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.PROGRAMMER.ToString())
            {
                Invoke(() => MainForm.Instance.DestTextBox.Enabled = false);
                Invoke(() => MainForm.Instance.OptimaSOATextBox.Enabled = false);
            }
        }

        public static void OpenLogsDirectory()
        {
            string logsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OSync");
            if (!Directory.Exists(logsDirectory))
            {
                Logger.Write(LogEventLevel.Error, Messages.LOGS_DIRECTORY_NOT_EXIST);
                MessageBox.Show(Messages.LOGS_DIRECTORY_NOT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(EXPLORER_PATH, logsDirectory);
        }

        public static void OpenUserManual()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(USER_MANUAL) { UseShellExecute = true });
        }

        public static void DisableElementsWhileProgrammer()
        {
            bool state;
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.PROGRAMMER.ToString())
            {
                state = false;
                SetStateOfRunOptimaCheckBox(state);
            }
            else
            {
                state = true;
            }

            MainForm.Instance.SOACheckBox.Checked = false;
            MainForm.Instance.SOACheckBox.Enabled = state;
            MainForm.Instance.RunOptimaCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima"));
            MainForm.Instance.RunOptimaCheckBox.Enabled = state;
            MainForm.Instance.destDirectoryLabel.Enabled = state;
            MainForm.Instance.DestTextBox.Enabled = state;
            MainForm.Instance.buttonDestinationDirectory.Enabled = state;
            MainForm.Instance.soaDestDirectoryLabel.Enabled = state;
            MainForm.Instance.OptimaSOATextBox.Enabled = state;
            MainForm.Instance.buttonOptimaSOADirectory.Enabled = state;
        }

        private static void SetStateOfRunOptimaCheckBox(bool state)
        {
            MainForm.Instance.RunOptimaCheckBox.Checked = state;
            MainForm.Instance.RunOptimaCheckBox.Enabled = state;
            AppConfigHelper.SetConfigValue("RunOptima", state.ToString());
        }
    }
}
