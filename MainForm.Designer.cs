﻿
namespace OptimaSync
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SyncTab = new System.Windows.Forms.TabPage();
            this.labelProgress = new System.Windows.Forms.Label();
            this.SOACheckBox = new System.Windows.Forms.CheckBox();
            this.downloadBuildButton = new System.Windows.Forms.Button();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.notificationGroupBox = new System.Windows.Forms.GroupBox();
            this.registeredOptimaNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.newBuildNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.RunOptimaCheckBox = new System.Windows.Forms.CheckBox();
            this.programmerCheckbox = new System.Windows.Forms.CheckBox();
            this.DestTextBox = new System.Windows.Forms.TextBox();
            this.buttonOptimaSOADirectory = new System.Windows.Forms.Button();
            this.OptimaSOATextBox = new System.Windows.Forms.TextBox();
            this.buttonDestinationDirectory = new System.Windows.Forms.Button();
            this.destDirectoryLabel = new System.Windows.Forms.Label();
            this.soaDestDirectoryLabel = new System.Windows.Forms.Label();
            this.HelpTab = new System.Windows.Forms.TabPage();
            this.authorLabel = new System.Windows.Forms.Label();
            this.openManualButton = new System.Windows.Forms.Button();
            this.openLogsButton = new System.Windows.Forms.Button();
            this.versionLabelValue = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.SyncTab.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.notificationGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.HelpTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SyncTab);
            this.tabControl1.Controls.Add(this.SettingsTab);
            this.tabControl1.Controls.Add(this.HelpTab);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(306, 238);
            this.tabControl1.TabIndex = 0;
            // 
            // SyncTab
            // 
            this.SyncTab.Controls.Add(this.labelProgress);
            this.SyncTab.Controls.Add(this.SOACheckBox);
            this.SyncTab.Controls.Add(this.downloadBuildButton);
            this.SyncTab.Location = new System.Drawing.Point(4, 24);
            this.SyncTab.Name = "SyncTab";
            this.SyncTab.Padding = new System.Windows.Forms.Padding(3);
            this.SyncTab.Size = new System.Drawing.Size(298, 210);
            this.SyncTab.TabIndex = 0;
            this.SyncTab.Text = "Sync";
            this.SyncTab.UseVisualStyleBackColor = true;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(13, 76);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(170, 15);
            this.labelProgress.TabIndex = 2;
            this.labelProgress.Text = "Status: OSA gotowa do pracy :)";
            // 
            // SOACheckBox
            // 
            this.SOACheckBox.AutoSize = true;
            this.SOACheckBox.Location = new System.Drawing.Point(13, 12);
            this.SOACheckBox.Name = "SOACheckBox";
            this.SOACheckBox.Size = new System.Drawing.Size(164, 19);
            this.SOACheckBox.TabIndex = 1;
            this.SOACheckBox.Text = "Kompilacja z obsługą SOA";
            this.SOACheckBox.UseVisualStyleBackColor = true;
            // 
            // downloadBuildButton
            // 
            this.downloadBuildButton.Location = new System.Drawing.Point(13, 37);
            this.downloadBuildButton.Name = "downloadBuildButton";
            this.downloadBuildButton.Size = new System.Drawing.Size(116, 23);
            this.downloadBuildButton.TabIndex = 0;
            this.downloadBuildButton.Text = "Pobierz kompilację";
            this.downloadBuildButton.UseVisualStyleBackColor = true;
            this.downloadBuildButton.Click += new System.EventHandler(this.DownloadBuildButton_Click);
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.notificationGroupBox);
            this.SettingsTab.Controls.Add(this.generalGroupBox);
            this.SettingsTab.Location = new System.Drawing.Point(4, 24);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(298, 210);
            this.SettingsTab.TabIndex = 1;
            this.SettingsTab.Text = "Ustawienia";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // notificationGroupBox
            // 
            this.notificationGroupBox.Controls.Add(this.registeredOptimaNotificationCheckBox);
            this.notificationGroupBox.Controls.Add(this.newBuildNotificationCheckBox);
            this.notificationGroupBox.Location = new System.Drawing.Point(0, 146);
            this.notificationGroupBox.Name = "notificationGroupBox";
            this.notificationGroupBox.Size = new System.Drawing.Size(295, 64);
            this.notificationGroupBox.TabIndex = 12;
            this.notificationGroupBox.TabStop = false;
            this.notificationGroupBox.Text = "Powiadomienia";
            // 
            // registeredOptimaNotificationCheckBox
            // 
            this.registeredOptimaNotificationCheckBox.AutoSize = true;
            this.registeredOptimaNotificationCheckBox.Location = new System.Drawing.Point(130, 23);
            this.registeredOptimaNotificationCheckBox.Name = "registeredOptimaNotificationCheckBox";
            this.registeredOptimaNotificationCheckBox.Size = new System.Drawing.Size(124, 19);
            this.registeredOptimaNotificationCheckBox.TabIndex = 1;
            this.registeredOptimaNotificationCheckBox.Text = "Zarejestrowanie O!";
            this.registeredOptimaNotificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // newBuildNotificationCheckBox
            // 
            this.newBuildNotificationCheckBox.AutoSize = true;
            this.newBuildNotificationCheckBox.Location = new System.Drawing.Point(6, 23);
            this.newBuildNotificationCheckBox.Name = "newBuildNotificationCheckBox";
            this.newBuildNotificationCheckBox.Size = new System.Drawing.Size(118, 19);
            this.newBuildNotificationCheckBox.TabIndex = 0;
            this.newBuildNotificationCheckBox.Text = "Nowa kompilacja";
            this.newBuildNotificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.RunOptimaCheckBox);
            this.generalGroupBox.Controls.Add(this.programmerCheckbox);
            this.generalGroupBox.Controls.Add(this.DestTextBox);
            this.generalGroupBox.Controls.Add(this.buttonOptimaSOADirectory);
            this.generalGroupBox.Controls.Add(this.OptimaSOATextBox);
            this.generalGroupBox.Controls.Add(this.buttonDestinationDirectory);
            this.generalGroupBox.Controls.Add(this.destDirectoryLabel);
            this.generalGroupBox.Controls.Add(this.soaDestDirectoryLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(0, 0);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(295, 140);
            this.generalGroupBox.TabIndex = 11;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "Ogólne";
            // 
            // RunOptimaCheckBox
            // 
            this.RunOptimaCheckBox.AutoSize = true;
            this.RunOptimaCheckBox.Location = new System.Drawing.Point(102, 21);
            this.RunOptimaCheckBox.Name = "RunOptimaCheckBox";
            this.RunOptimaCheckBox.Size = new System.Drawing.Size(159, 19);
            this.RunOptimaCheckBox.TabIndex = 11;
            this.RunOptimaCheckBox.Text = "Uruchom O! po pobraniu";
            this.RunOptimaCheckBox.UseVisualStyleBackColor = true;
            this.RunOptimaCheckBox.Click += new System.EventHandler(this.RunOptimaCheckBox_Click);
            // 
            // programmerCheckbox
            // 
            this.programmerCheckbox.AutoSize = true;
            this.programmerCheckbox.Location = new System.Drawing.Point(6, 21);
            this.programmerCheckbox.Name = "programmerCheckbox";
            this.programmerCheckbox.Size = new System.Drawing.Size(90, 19);
            this.programmerCheckbox.TabIndex = 10;
            this.programmerCheckbox.Text = "Programista";
            this.programmerCheckbox.UseVisualStyleBackColor = true;
            this.programmerCheckbox.Click += new System.EventHandler(this.ProgrammerCheckbox_Click);
            // 
            // DestTextBox
            // 
            this.DestTextBox.Location = new System.Drawing.Point(6, 65);
            this.DestTextBox.Name = "DestTextBox";
            this.DestTextBox.Size = new System.Drawing.Size(233, 23);
            this.DestTextBox.TabIndex = 1;
            // 
            // buttonOptimaSOADirectory
            // 
            this.buttonOptimaSOADirectory.Location = new System.Drawing.Point(246, 109);
            this.buttonOptimaSOADirectory.Name = "buttonOptimaSOADirectory";
            this.buttonOptimaSOADirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonOptimaSOADirectory.TabIndex = 8;
            this.buttonOptimaSOADirectory.Text = "...";
            this.buttonOptimaSOADirectory.UseVisualStyleBackColor = true;
            this.buttonOptimaSOADirectory.Click += new System.EventHandler(this.ButtonOptimaSOADirectory_Click);
            // 
            // OptimaSOATextBox
            // 
            this.OptimaSOATextBox.Location = new System.Drawing.Point(6, 109);
            this.OptimaSOATextBox.Name = "OptimaSOATextBox";
            this.OptimaSOATextBox.Size = new System.Drawing.Size(233, 23);
            this.OptimaSOATextBox.TabIndex = 2;
            // 
            // buttonDestinationDirectory
            // 
            this.buttonDestinationDirectory.Location = new System.Drawing.Point(246, 65);
            this.buttonDestinationDirectory.Name = "buttonDestinationDirectory";
            this.buttonDestinationDirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonDestinationDirectory.TabIndex = 7;
            this.buttonDestinationDirectory.Text = "...";
            this.buttonDestinationDirectory.UseVisualStyleBackColor = true;
            this.buttonDestinationDirectory.Click += new System.EventHandler(this.ButtonDestinationDirectory_Click);
            // 
            // destDirectoryLabel
            // 
            this.destDirectoryLabel.AutoSize = true;
            this.destDirectoryLabel.Location = new System.Drawing.Point(6, 47);
            this.destDirectoryLabel.Name = "destDirectoryLabel";
            this.destDirectoryLabel.Size = new System.Drawing.Size(94, 15);
            this.destDirectoryLabel.TabIndex = 4;
            this.destDirectoryLabel.Text = "Folder docelowy";
            // 
            // soaDestDirectoryLabel
            // 
            this.soaDestDirectoryLabel.AutoSize = true;
            this.soaDestDirectoryLabel.Location = new System.Drawing.Point(6, 91);
            this.soaDestDirectoryLabel.Name = "soaDestDirectoryLabel";
            this.soaDestDirectoryLabel.Size = new System.Drawing.Size(223, 15);
            this.soaDestDirectoryLabel.TabIndex = 5;
            this.soaDestDirectoryLabel.Text = "Folder instalacyjny Comarch ERP Optima";
            // 
            // HelpTab
            // 
            this.HelpTab.Controls.Add(this.authorLabel);
            this.HelpTab.Controls.Add(this.openManualButton);
            this.HelpTab.Controls.Add(this.openLogsButton);
            this.HelpTab.Controls.Add(this.versionLabelValue);
            this.HelpTab.Controls.Add(this.versionLabel);
            this.HelpTab.Location = new System.Drawing.Point(4, 24);
            this.HelpTab.Name = "HelpTab";
            this.HelpTab.Size = new System.Drawing.Size(298, 210);
            this.HelpTab.TabIndex = 2;
            this.HelpTab.Text = "Pomoc";
            this.HelpTab.UseVisualStyleBackColor = true;
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.authorLabel.Location = new System.Drawing.Point(10, 191);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(176, 12);
            this.authorLabel.TabIndex = 4;
            this.authorLabel.Text = "Bartlomiej.Komendarczuk@comarch.pl";
            // 
            // openManualButton
            // 
            this.openManualButton.Location = new System.Drawing.Point(10, 45);
            this.openManualButton.Name = "openManualButton";
            this.openManualButton.Size = new System.Drawing.Size(136, 23);
            this.openManualButton.TabIndex = 3;
            this.openManualButton.Text = "Otwórz instrukcję";
            this.openManualButton.UseVisualStyleBackColor = true;
            this.openManualButton.Click += new System.EventHandler(this.OpenManualButton_Click);
            // 
            // openLogsButton
            // 
            this.openLogsButton.Location = new System.Drawing.Point(10, 16);
            this.openLogsButton.Name = "openLogsButton";
            this.openLogsButton.Size = new System.Drawing.Size(136, 23);
            this.openLogsButton.TabIndex = 2;
            this.openLogsButton.Text = "Otwórz folder z logami";
            this.openLogsButton.UseVisualStyleBackColor = true;
            this.openLogsButton.Click += new System.EventHandler(this.OpenLogsButton_Click);
            // 
            // versionLabelValue
            // 
            this.versionLabelValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabelValue.AutoSize = true;
            this.versionLabelValue.Location = new System.Drawing.Point(237, 188);
            this.versionLabelValue.Name = "versionLabelValue";
            this.versionLabelValue.Size = new System.Drawing.Size(49, 15);
            this.versionLabelValue.TabIndex = 1;
            this.versionLabelValue.Text = "0000.0.0";
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(192, 188);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(48, 15);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Wersja: ";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 236);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(317, 275);
            this.MinimumSize = new System.Drawing.Size(317, 275);
            this.Name = "MainForm";
            this.Text = "OptimaSync";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.SyncTab.ResumeLayout(false);
            this.SyncTab.PerformLayout();
            this.SettingsTab.ResumeLayout(false);
            this.notificationGroupBox.ResumeLayout(false);
            this.notificationGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.HelpTab.ResumeLayout(false);
            this.HelpTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SyncTab;
        private System.Windows.Forms.TabPage SettingsTab;
        public System.Windows.Forms.Button downloadBuildButton;
        private System.Windows.Forms.TabPage HelpTab;
        public System.Windows.Forms.CheckBox SOACheckBox;
        public System.Windows.Forms.Label destDirectoryLabel;
        public System.Windows.Forms.TextBox OptimaSOATextBox;
        public System.Windows.Forms.TextBox DestTextBox;
        public System.Windows.Forms.Label soaDestDirectoryLabel;
        public System.Windows.Forms.Button buttonOptimaSOADirectory;
        public System.Windows.Forms.Button buttonDestinationDirectory;
        public System.Windows.Forms.Label labelProgress;
        public System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label versionLabelValue;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button openManualButton;
        private System.Windows.Forms.Button openLogsButton;
        private System.Windows.Forms.Label authorLabel;
        public System.Windows.Forms.CheckBox programmerCheckbox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        public System.Windows.Forms.CheckBox RunOptimaCheckBox;
        private System.Windows.Forms.GroupBox notificationGroupBox;
        private System.Windows.Forms.CheckBox registeredOptimaNotificationCheckBox;
        private System.Windows.Forms.CheckBox newBuildNotificationCheckBox;
    }
}

