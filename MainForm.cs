using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;
using OptimaSync.Constants;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        CompilationSync compilationSync = new CompilationSync();
        SyncUI syncUI = new SyncUI();
        AppSettings appSettings = new AppSettings();
        public MainForm()
        {
            InitializeComponent();
            this.SourcePathTextBox.Text = Properties.Settings.Default.BuildSourcePath;
            this.DestTextBox.Text = Properties.Settings.Default.BuildDestPath;
            this.OptimaSOATextBox.Text = Properties.Settings.Default.BuildSOAPath;
        }

        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            if (SOACheckBox.Checked && string.IsNullOrEmpty(OptimaSOATextBox.Text))
            {
                MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.SOA_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                compilationSync.DownloadLatestCompilation();
            }
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
                MessageBox.Show(Messages.BUILD_PATH_CANNOT_BE_EMPTY, Messages.BUILD_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
             appSettings.SetPaths(SourcePathTextBox.Text, DestTextBox.Text, OptimaSOATextBox.Text);
            }
        }
    }
}
