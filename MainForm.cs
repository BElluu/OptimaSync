using AutoUpdaterDotNET;
using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.Constant;
using Serilog;
using System.ComponentModel;
using OptimaSync.Common;
using System.Configuration;
using OptimaSync.Helper;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        private static string AUTO_UPDATE_CONFIG = "https://osync.devopsowy.pl/AutoUpdater.xml";

        BuildSyncService buildSyncService;
        SyncUI syncUI;
        ValidatorUI validatorUI;
        SearchBuildService searchBuildService;

        private static MainForm _instance;
        public MainForm(BuildSyncService buildSyncService, SyncUI syncUI, ValidatorUI validatorUI, SearchBuildService searchBuildService)
        {
            this.buildSyncService = buildSyncService;
            this.syncUI = syncUI;
            this.validatorUI = validatorUI;
            this.searchBuildService = searchBuildService;

            InitializeComponent();
            this.DestTextBox.Text = AppConfigHelper.GetConfigValue("Destination");
            this.OptimaSOATextBox.Text = AppConfigHelper.GetConfigValue("SOADestination");
            this.versionLabelValue.Text = syncUI.GetAppVersion();
            this.RunOptimaCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima"));
            this.newVersionNotificationCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("AutoCheckVersion"));
            this.turnOnSoundNotificationCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("NotificationSound"));
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.PROGRAMMER.ToString())
            {
                this.programmerCheckbox.Checked = true;
            }
            _instance = this;
            AutoUpdater.Start(AUTO_UPDATE_CONFIG);
            InitCheckVersionTimer();
        }

        public string ProgressLabelStatus
        {
            get { return labelProgress.Text; }
            set { labelProgress.Text = value; }
        }

        public static MainForm Instance { get { return _instance; } }

        private void MainForm_Shown(Object sender, EventArgs e)
        {
            syncUI.DisableElementsWhileProgrammer();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.Visible = true;
                SyncUI.Invoke(() => Notification(Messages.OSA_WORKING_IN_BACKGROUND, NotificationForm.enumType.Informaton));
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void showNotifyIconMenu_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        public void showUpTrayWhenNewInstance()
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void downloadNotifyIconMenu_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void exitNotifyIconMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void DownloadBuildButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void ButtonDestinationDirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(DestTextBox);
            AppConfigHelper.SetConfigValue("Destination", DestTextBox.Text);
        }

        private void ButtonOptimaSOADirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(OptimaSOATextBox);
            AppConfigHelper.SetConfigValue("SOADestination", OptimaSOATextBox.Text);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            buildSyncService.PrepareOptimaBuild(AppConfigHelper.GetConfigValue("DownloadType"));
        }

        private void OpenLogsButton_Click(object sender, EventArgs e)
        {
            try
            {
                syncUI.OpenLogsDirectory();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Notification(string message, NotificationForm.enumType notificationType)
        {
            NotificationForm notificationForm = new NotificationForm();
            notificationForm.showNotification(message, notificationType);

            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("NotificationSound")) == true)
            {
                SoundPlayer.PlayNotificationSound();
            }
        }

        private void OpenManualButton_Click(object sender, EventArgs e)
        {
            syncUI.OpenUserManual();
        }

        private void ProgrammerCheckbox_Click(object sender, EventArgs e)
        {
            if (programmerCheckbox.Checked)
            {
                AppConfigHelper.SetConfigValue("DownloadType", DownloadTypeEnum.PROGRAMMER.ToString());
                syncUI.DisableElementsWhileProgrammer();
            }
            else
            {
                syncUI.DisableElementsWhileProgrammer();
            }
        }

        private void RunOptimaCheckBox_Click(object sender, EventArgs e)
        {
            if (RunOptimaCheckBox.Checked)
            {
                AppConfigHelper.SetConfigValue("RunOptima", "true");
            }
            else
            {
                AppConfigHelper.SetConfigValue("RunOptima", "false");
            }
        }

        private void InitCheckVersionTimer()
        {
            Timer checkVersionTimer = new Timer();
            checkVersionTimer.Tick += new EventHandler(CheckVersionTimer);
            checkVersionTimer.Interval = 1000 * 60 * 20;
            checkVersionTimer.Start();
        }

        private void CheckVersionTimer(object sender, EventArgs e)
        {
            if (!backgroundWorkerNotification.IsBusy)
            {
            backgroundWorkerNotification.RunWorkerAsync();
            }
        }

        private void newVersionNotificationCheckBox_Click(object sender, EventArgs e)
        {
            if (newVersionNotificationCheckBox.Checked)
            {
                AppConfigHelper.SetConfigValue("AutoCheckVersion", "true");
            }
            else
            {
                AppConfigHelper.SetConfigValue("AutoCheckVersion", "false");
            }
        }

        private void turnOnSoundNotificationCheckBox_Click(object sender, EventArgs e)
        {
            if (turnOnSoundNotificationCheckBox.Checked)
            {
                AppConfigHelper.SetConfigValue("NotificationSound", "true");
            }
            else
            {
                AppConfigHelper.SetConfigValue("NotificationSound", "false");
            }
        }

        private void backgroundWorkerNotification_DoWork(object sender, DoWorkEventArgs e)
        {
            searchBuildService.AutoCheckNewVersion();
        }
    }
}
