using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;
using OptimaSync.Constants;
using Serilog;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        BuildSync buildSync = new BuildSync();
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
            if (SOACheckBox.Checked)
            {
                if (string.IsNullOrEmpty(OptimaSOATextBox.Text))
                {
                    MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.SOA_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                }
                else
                {
                    buildSync.DownloadLatestBuildWithSOA();
                }
            }
            if (!SOACheckBox.Checked)
            {
                if (string.IsNullOrEmpty(DestTextBox.Text))
                {
                    MessageBox.Show(Messages.DEST_PATH_CANNOT_BE_EMPTY, Messages.DEST_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                }
                else
                {
                    buildSync.DownloadLatestBuild();
                }
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
                Log.Error(Messages.BUILD_PATH_CANNOT_BE_EMPTY);
            }
            else
            {
             appSettings.SetPaths(SourcePathTextBox.Text.ToString(), DestTextBox.Text, OptimaSOATextBox.Text);
            }
        }
    }
}
