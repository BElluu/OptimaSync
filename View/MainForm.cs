using AutoUpdaterDotNET;
using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;
using OptimaSync.Constant;
using Serilog;
using System.ComponentModel;
using OptimaSync.View;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        BuildSyncService buildSyncService = new BuildSyncService();
        SyncUI syncUI = new SyncUI();
        AppSettings appSettings = new AppSettings();

        private static MainForm _instance;
        public MainForm()
        {
            InitializeComponent();
            this.SourcePathTextBox.Text = Properties.Settings.Default.BuildSourcePath;
            this.DestTextBox.Text = Properties.Settings.Default.BuildDestPath;
            this.OptimaSOATextBox.Text = Properties.Settings.Default.BuildSOAPath;
            this.versionLabelValue.Text = syncUI.GetAppVersion();
            _instance = this;
            AutoUpdater.Start("https://osync.devopsowy.pl/AutoUpdater.xml");
        }

        public string progressLabelStatus
        {
            get { return labelProgress.Text; }
            set { labelProgress.Text = value; }
        }

        public static MainForm Instance { get { return _instance; } }

        public void Alert(string alertMsg)
        {
            AlertForm alertForm = new AlertForm();
            alertForm.showAlert(alertMsg);
        }


        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
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
                MessageBox.Show(Messages.BUILD_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(Messages.BUILD_PATH_CANNOT_BE_EMPTY);
            }
            else
            {
                appSettings.SetPaths(SourcePathTextBox.Text.ToString(), DestTextBox.Text, OptimaSOATextBox.Text);
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (SOACheckBox.Checked)
            {
                if (string.IsNullOrEmpty(OptimaSOATextBox.Text))
                {
                    MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                }
                else
                {
                    buildSyncService.PrepareOptimaBuild(true);
                }
            }
            if (!SOACheckBox.Checked)
            {
                if (string.IsNullOrEmpty(DestTextBox.Text))
                {
                    MessageBox.Show(Messages.DEST_PATH_CANNOT_BE_EMPTY, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                }
                else
                {
                    buildSyncService.PrepareOptimaBuild(false);
                }
            }
        }

        private void openLogsButton_Click(object sender, EventArgs e)
        {
            try
            {
                syncUI.OpenLogsDirectory();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                MessageBox.Show(ex.Message, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openManualButton_Click(object sender, EventArgs e)
        {
            //syncUI.OpenUserManual();
            this.Alert("Pojawiła się nowa kompilacja!");
        }
    }
}
