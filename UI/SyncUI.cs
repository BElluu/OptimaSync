using System;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class SyncUI
    {
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
    }
}
