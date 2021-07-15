using AutoUpdaterDotNET;
using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.Constant;
using Serilog;
using System.ComponentModel;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        BuildSyncService buildSyncService = new BuildSyncService();
        SyncUI syncUI = new SyncUI();
        ValidatorUI validatorUI = new ValidatorUI();

        private static MainForm _instance;
        public MainForm()
        {
            InitializeComponent();
            this.DestTextBox.Text = Properties.Settings.Default.BuildDestPath;
            this.OptimaSOATextBox.Text = Properties.Settings.Default.BuildSOAPath;
            this.versionLabelValue.Text = syncUI.GetAppVersion();
            this.programmerCheckbox.Checked = Properties.Settings.Default.IsProgrammer;
            _instance = this;
            AutoUpdater.Start("https://osync.devopsowy.pl/AutoUpdater.xml");
        }

        public string progressLabelStatus
        {
            get { return labelProgress.Text; }
            set { labelProgress.Text = value; }
        }

        public static MainForm Instance { get { return _instance; } }

        private void MainForm_Shown(Object sender, EventArgs e)
        {
            syncUI.DisableElementsWhileProgrammer(Properties.Settings.Default.IsProgrammer);
        }


        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void buttonDestinationDirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(DestTextBox);
            Properties.Settings.Default.BuildDestPath = DestTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonOptimaSOADirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(OptimaSOATextBox);
            Properties.Settings.Default.BuildSOAPath = OptimaSOATextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            buildSyncService.PrepareOptimaBuild(validatorUI.WithSOASupport(), validatorUI.isProgrammer());
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
            syncUI.OpenUserManual();
        }

        private void programmerCheckbox_Click(object sender, EventArgs e)
        {
            if (programmerCheckbox.Checked)
            {
                Properties.Settings.Default.IsProgrammer = true;
                Properties.Settings.Default.Save();
                syncUI.DisableElementsWhileProgrammer(true);
            }
            else
            {
                Properties.Settings.Default.IsProgrammer = false;
                Properties.Settings.Default.Save();
                syncUI.DisableElementsWhileProgrammer(false);
            }
        }
    }
}
