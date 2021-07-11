using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;
using OptimaSync.Constant;
using Serilog;
using System.ComponentModel;

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
        }

        public string progressLabelStatus
        {
            get { return labelProgress.Text; }
            set { labelProgress.Text = value; }
        }

        public static MainForm Instance { get { return _instance; } }


        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            /*            if (SOACheckBox.Checked)
                        {
                            if (string.IsNullOrEmpty(OptimaSOATextBox.Text))
                            {
                                MessageBox.Show(Messages.SOA_PATH_CANNOT_BE_EMPTY, Messages.SOA_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                MessageBox.Show(Messages.DEST_PATH_CANNOT_BE_EMPTY, Messages.DEST_PATH_CANNOT_BE_EMPTY_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                            }
                            else
                            {

                                buildSyncService.PrepareOptimaBuild(false);
                            }
                        }*/
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

        /*        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
                {
                    downloadProgressBar.Value = e.ProgressPercentage;
                    labelProgress.Text = e.ProgressPercentage.ToString() + "%";

                    labelProgress.Invoke(new Action(() => { labelProgress.Text = e.ToString(); }));
                    labelProgress.Invoke(new Action(() => { labelProgress.Refresh(); }));


                    private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
                    {
                    }*/
    }
}
