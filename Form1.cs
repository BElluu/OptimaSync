using OptimaSync.Compilation;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;

namespace OptimaSync
{
    public partial class Form1 : Form
    {
        CompilationSync compilationSync = new CompilationSync();
        SyncUI syncUI = new SyncUI();
        AppSettings appSettings = new AppSettings();
        public Form1()
        {
            InitializeComponent();
            this.SourcePathTextBox.Text = Properties.Settings.Default.BuildSourcePath;
            this.DestTextBox.Text = Properties.Settings.Default.BuildDestPath;
            this.OptimaSOATextBox.Text = Properties.Settings.Default.BuildSOAPath;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            if (SOACheckBox.Checked && string.IsNullOrEmpty(OptimaSOATextBox.Text))
            {
                MessageBox.Show("Chcesz wykorzystać SOA. Ścieżka instalacyjna Optimy musi być uzupełniona!");
            }
            else
            {
                compilationSync.DownloadLatestCompilation();
            }
        }

        private void SyncTab_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonSourceDirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(SourcePathTextBox);
        }

        private void buttonDestinationDirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(DestTextBox);
        }

        private void buttonOptimaSOADirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(OptimaSOATextBox);
        }

        private void saveSettingsButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SourcePathTextBox.Text))
            {
                MessageBox.Show("Ścieżka kompilacji nie może być pusta!");
            }
            else
            {
                appSettings.SetPaths(SourcePathTextBox.Text, DestTextBox.Text, OptimaSOATextBox.Text);
            }
        }
    }
}
