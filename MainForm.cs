using AutoUpdaterDotNET;
using OptimaSync.Service;
using System;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.Constant;
using System.ComponentModel;
using OptimaSync.Common;
using OptimaSync.Helper;
using Serilog.Events;

namespace OptimaSync
{
    public partial class MainForm : Form
    {
        private static string AUTO_UPDATE_CONFIG = "https://osync.devopsowy.pl/AutoUpdater.xml";

        private readonly DownloadOptimaService downloaderService;

        private static MainForm _instance = null;
        public MainForm(DownloadOptimaService buildSyncService)
        {
            downloaderService = buildSyncService;

            InitializeComponent();
            SetValuesFromConfig();
            FillProductionVersionList();
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
            SyncUI.DisableElementsWhileProgrammer();
            prodVersionDropMenu.Enabled = false;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.Visible = true;
                SyncUI.Invoke(() => Notification(Messages.OSA_WORKING_IN_BACKGROUND, NotificationForm.notificationType.Informaton));
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
            backgroundWorkerBuild.RunWorkerAsync();
        }

        private void exitNotifyIconMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void DownloadBuildButton_Click(object sender, EventArgs e)
        {
            backgroundWorkerBuild.RunWorkerAsync();
        }

        private void ButtonDestinationDirectory_Click(object sender, EventArgs e)
        {
            SyncUI.PathToTextbox(DestTextBox);
            AppConfigHelper.SetConfigValue("Destination", DestTextBox.Text);
        }

        private void ButtonOptimaSOADirectory_Click(object sender, EventArgs e)
        {
            SyncUI.PathToTextbox(OptimaSOATextBox);
            AppConfigHelper.SetConfigValue("SOADestination", OptimaSOATextBox.Text);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool buildVersion = true;
            string prodVersionPath = string.Empty;
            if(prodRadio.Checked)
            {
                buildVersion = false;
            }
            this.Invoke(new Action(() =>
            {
                prodVersionPath = prodVersionDropMenu.SelectedValue.ToString();
            }));
            downloaderService.GetOptima(buildVersion, prodVersionPath, Convert.ToBoolean(AppConfigHelper.GetConfigValue("DownloadEDeclaration")));
        }

        private void OpenLogsButton_Click(object sender, EventArgs e)
        {
            try
            {
                SyncUI.OpenLogsDirectory();
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                MessageBox.Show(ex.Message, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Notification(string message, NotificationForm.notificationType notificationType)
        {
            NotificationForm notificationForm = new NotificationForm();
            notificationForm.showNotification(message, notificationType);

            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("NotificationSound")))
            {
                SoundPlayer.PlayNotificationSound();
            }
        }

        private void OpenManualButton_Click(object sender, EventArgs e)
        {
            SyncUI.OpenUserManual();
        }

        private void ProgrammerCheckbox_Click(object sender, EventArgs e)
        {
            if (programmerCheckbox.Checked)
            {
                AppConfigHelper.SetConfigValue("DownloadType", DownloadType.PROGRAMMER.ToString());
                SyncUI.DisableElementsWhileProgrammer();
            }
            else
            {
                AppConfigHelper.SetConfigValue("DownloadType", DownloadType.BASIC.ToString());
                SyncUI.DisableElementsWhileProgrammer();
            }
        }

        private void SOACheckBox_Click(object sender, EventArgs e)
        {
            if (SOACheckBox.Checked)
            {
                AppConfigHelper.SetConfigValue("DownloadType", DownloadType.SOA.ToString());
            } else
            {
                AppConfigHelper.SetConfigValue("DownloadType", DownloadType.BASIC.ToString());
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
            SearchOptimaBuildService.AutoCheckNewVersion();
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

        private void FillProductionVersionList()
        {
            var dict = DownloadOptimaService.GetListOfProd();
            prodVersionDropMenu.DropDownStyle = ComboBoxStyle.DropDownList;
            prodVersionDropMenu.DataSource = new BindingSource(dict, null);
            prodVersionDropMenu.DisplayMember = "Key";
            prodVersionDropMenu.ValueMember = "Value";
        }

        private void SetValuesFromConfig()
        {
            this.DestTextBox.Text = AppConfigHelper.GetConfigValue("Destination");
            this.OptimaSOATextBox.Text = AppConfigHelper.GetConfigValue("SOADestination");
            this.versionLabelValue.Text = SyncUI.GetAppVersion();
            this.RunOptimaCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima"));
            this.newVersionNotificationCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("AutoCheckVersion"));
            this.turnOnSoundNotificationCheckBox.Checked = Convert.ToBoolean(AppConfigHelper.GetConfigValue("NotificationSound"));
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.PROGRAMMER.ToString())
                this.programmerCheckbox.Checked = true;
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.SOA.ToString())
                this.SOACheckBox.Checked = true;
        }

        private void buildRadio_CheckedChanged(object sender, EventArgs e)
        {
            prodVersionDropMenu.Enabled = false;
        }

        private void prodRadio_CheckedChanged(object sender, EventArgs e)
        {
            prodVersionDropMenu.Enabled = true;
        }

        private void eDeclarationCheckBox_Click(object sender, EventArgs e)
        {
            if (eDeclarationCheckBox.Checked)
            {
                AppConfigHelper.SetConfigValue("DownloadEDeclaration", "true");
            }
            else
            {
                AppConfigHelper.SetConfigValue("DownloadEDeclaration", "false");
            }
        }
    }
}
