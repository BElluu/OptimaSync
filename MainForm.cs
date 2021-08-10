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
        private static string AUTO_UPDATE_CONFIG = "https://osync.devopsowy.pl/AutoUpdater.xml";

        BuildSyncService buildSyncService = new BuildSyncService();
        SyncUI syncUI = new SyncUI();
        ValidatorUI validatorUI = new ValidatorUI();
        SearchBuildService searchBuildService = new SearchBuildService();

        private static MainForm _instance;
        public MainForm()
        {
            InitializeComponent();
            this.DestTextBox.Text = Properties.Settings.Default.BuildDestPath;
            this.OptimaSOATextBox.Text = Properties.Settings.Default.BuildSOAPath;
            this.versionLabelValue.Text = syncUI.GetAppVersion();
            this.programmerCheckbox.Checked = Properties.Settings.Default.IsProgrammer;
            this.RunOptimaCheckBox.Checked = Properties.Settings.Default.RunOptima;
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
            syncUI.DisableElementsWhileProgrammer(Properties.Settings.Default.IsProgrammer);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.Visible = true;
                SyncUI.Invoke(() => Notification("OptimaSync działa w tle", NotificationForm.enumType.Informaton));
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
            Properties.Settings.Default.BuildDestPath = DestTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void ButtonOptimaSOADirectory_Click(object sender, EventArgs e)
        {
            syncUI.PathToTextbox(OptimaSOATextBox);
            Properties.Settings.Default.BuildSOAPath = OptimaSOATextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            buildSyncService.PrepareOptimaBuild(validatorUI.WithSOASupport(), validatorUI.isProgrammer());
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
        }

        private void OpenManualButton_Click(object sender, EventArgs e)
        {
            syncUI.OpenUserManual();
        }

        private void ProgrammerCheckbox_Click(object sender, EventArgs e)
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

        private void RunOptimaCheckBox_Click(object sender, EventArgs e)
        {
            if (RunOptimaCheckBox.Checked)
            {
                Properties.Settings.Default.RunOptima = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.RunOptima = false;
                Properties.Settings.Default.Save();
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
            if (!backgroundWorker.IsBusy)
            {
                searchBuildService.AutoCheckNewVersion();
            }
        }
    }
}
