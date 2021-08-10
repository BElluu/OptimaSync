﻿using OptimaSync.Constant;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class SyncUI
    {
        static readonly string USER_MANUAL = "https://osync.devopsowy.pl/Instrukcja.pdf";
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
            if (Properties.Settings.Default.IsProgrammer == false)
            {
                Invoke(() => MainForm.Instance.downloadBuildButton.Enabled = state);
                Invoke(() => MainForm.Instance.SOACheckBox.Enabled = state);
                Invoke(() => MainForm.Instance.programmerCheckbox.Enabled = state);
                Invoke(() => MainForm.Instance.RunOptimaCheckBox.Enabled = state);
                Invoke(() => MainForm.Instance.DestTextBox.Enabled = state);
                Invoke(() => MainForm.Instance.OptimaSOATextBox.Enabled = state);
                Invoke(() => MainForm.Instance.buttonDestinationDirectory.Enabled = state);
                Invoke(() => MainForm.Instance.buttonOptimaSOADirectory.Enabled = state);
            }
            else
            {
                Invoke(() => MainForm.Instance.downloadBuildButton.Enabled = state);
                //Invoke(() => MainForm.Instance.SOACheckBox.Enabled = state);
                Invoke(() => MainForm.Instance.programmerCheckbox.Enabled = state);
               // Invoke(() => MainForm.Instance.RunOptimaCheckBox.Enabled = state);
            }
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
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(USER_MANUAL) { UseShellExecute = true });
        }

        public void DisableElementsWhileProgrammer(bool isProgrammer)
        {
            bool state;
            if (isProgrammer)
            {
                state = false;
                MainForm.Instance.RunOptimaCheckBox.Enabled = state;
                SetStateOfRunOptimaCheckBox(state);
            }
            else
            {
                state = true;
            }

            MainForm.Instance.SOACheckBox.Checked = false;
            MainForm.Instance.SOACheckBox.Enabled = state;
            MainForm.Instance.RunOptimaCheckBox.Checked = Properties.Settings.Default.RunOptima;
            MainForm.Instance.RunOptimaCheckBox.Enabled = state;
            MainForm.Instance.destDirectoryLabel.Enabled = state;
            MainForm.Instance.DestTextBox.Enabled = state;
            MainForm.Instance.buttonDestinationDirectory.Enabled = state;
            MainForm.Instance.soaDestDirectoryLabel.Enabled = state;
            MainForm.Instance.OptimaSOATextBox.Enabled = state;
            MainForm.Instance.buttonOptimaSOADirectory.Enabled = state;
        }

        private void SetStateOfRunOptimaCheckBox(bool state)
        {
            MainForm.Instance.RunOptimaCheckBox.Checked = state;
            Properties.Settings.Default.RunOptima = state;
            Properties.Settings.Default.Save();
        }
    }
}
