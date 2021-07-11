﻿using OptimaSync.Constant;
using Serilog;
using System;
using System.IO;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class SyncUI
    {
        static readonly string UserManual = "https://osync.devopsowy.pl/Instrukcja.pdf";
        public void PathToTextbox(TextBox textBox)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox.Text = String.Empty;
            textBox.AppendText(folderBrowserDialog.SelectedPath);
        }

        public void ChangeProgressLabel(string status)
        {
            MainForm.Instance.progressLabelStatus = "Status: " + status;
            MainForm.Instance.labelProgress.Refresh();
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
            MainForm.Instance.saveSettingsButton.Enabled = state;
            MainForm.Instance.SOACheckBox.Enabled = state;
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
    }
}
