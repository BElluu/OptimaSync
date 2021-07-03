
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SyncTab = new System.Windows.Forms.TabPage();
            this.SOACheckBox = new System.Windows.Forms.CheckBox();
            this.downloadBuildButton = new System.Windows.Forms.Button();
            this.SettingsTab = new System.Windows.Forms.TabPage();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.buttonOptimaSOADirectory = new System.Windows.Forms.Button();
            this.buttonDestinationDirectory = new System.Windows.Forms.Button();
            this.buttonSourceDirectory = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OptimaSOATextBox = new System.Windows.Forms.TextBox();
            this.DestTextBox = new System.Windows.Forms.TextBox();
            this.SourcePathTextBox = new System.Windows.Forms.TextBox();
            this.HelpTab = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SyncTab.SuspendLayout();
            this.SettingsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SyncTab);
            this.tabControl1.Controls.Add(this.SettingsTab);
            this.tabControl1.Controls.Add(this.HelpTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(799, 488);
            this.tabControl1.TabIndex = 0;
            // 
            // SyncTab
            // 
            this.SyncTab.Controls.Add(this.SOACheckBox);
            this.SyncTab.Controls.Add(this.downloadBuildButton);
            this.SyncTab.Location = new System.Drawing.Point(4, 24);
            this.SyncTab.Name = "SyncTab";
            this.SyncTab.Padding = new System.Windows.Forms.Padding(3);
            this.SyncTab.Size = new System.Drawing.Size(791, 460);
            this.SyncTab.TabIndex = 0;
            this.SyncTab.Text = "Sync";
            this.SyncTab.UseVisualStyleBackColor = true;
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
            this.downloadBuildButton.Click += new System.EventHandler(this.downloadBuildButton_Click);
            // 
            // SettingsTab
            // 
            this.SettingsTab.Controls.Add(this.saveSettingsButton);
            this.SettingsTab.Controls.Add(this.buttonOptimaSOADirectory);
            this.SettingsTab.Controls.Add(this.buttonDestinationDirectory);
            this.SettingsTab.Controls.Add(this.buttonSourceDirectory);
            this.SettingsTab.Controls.Add(this.label3);
            this.SettingsTab.Controls.Add(this.label2);
            this.SettingsTab.Controls.Add(this.label1);
            this.SettingsTab.Controls.Add(this.OptimaSOATextBox);
            this.SettingsTab.Controls.Add(this.DestTextBox);
            this.SettingsTab.Controls.Add(this.SourcePathTextBox);
            this.SettingsTab.Location = new System.Drawing.Point(4, 24);
            this.SettingsTab.Name = "SettingsTab";
            this.SettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.SettingsTab.Size = new System.Drawing.Size(791, 460);
            this.SettingsTab.TabIndex = 1;
            this.SettingsTab.Text = "Ustawienia";
            this.SettingsTab.UseVisualStyleBackColor = true;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Location = new System.Drawing.Point(30, 204);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(127, 23);
            this.saveSettingsButton.TabIndex = 9;
            this.saveSettingsButton.Text = "Zapisz Ustawienia";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // buttonOptimaSOADirectory
            // 
            this.buttonOptimaSOADirectory.Location = new System.Drawing.Point(252, 152);
            this.buttonOptimaSOADirectory.Name = "buttonOptimaSOADirectory";
            this.buttonOptimaSOADirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonOptimaSOADirectory.TabIndex = 8;
            this.buttonOptimaSOADirectory.Text = "...";
            this.buttonOptimaSOADirectory.UseVisualStyleBackColor = true;
            this.buttonOptimaSOADirectory.Click += new System.EventHandler(this.buttonOptimaSOADirectory_Click);
            // 
            // buttonDestinationDirectory
            // 
            this.buttonDestinationDirectory.Location = new System.Drawing.Point(252, 108);
            this.buttonDestinationDirectory.Name = "buttonDestinationDirectory";
            this.buttonDestinationDirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonDestinationDirectory.TabIndex = 7;
            this.buttonDestinationDirectory.Text = "...";
            this.buttonDestinationDirectory.UseVisualStyleBackColor = true;
            this.buttonDestinationDirectory.Click += new System.EventHandler(this.buttonDestinationDirectory_Click);
            // 
            // buttonSourceDirectory
            // 
            this.buttonSourceDirectory.Location = new System.Drawing.Point(252, 63);
            this.buttonSourceDirectory.Name = "buttonSourceDirectory";
            this.buttonSourceDirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonSourceDirectory.TabIndex = 6;
            this.buttonSourceDirectory.Text = "...";
            this.buttonSourceDirectory.UseVisualStyleBackColor = true;
            this.buttonSourceDirectory.Click += new System.EventHandler(this.buttonSourceDirectory_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Folder instalacyjny Comarch ERP Optima";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Folder docelowy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Folder z kompilacjami";
            // 
            // OptimaSOATextBox
            // 
            this.OptimaSOATextBox.Location = new System.Drawing.Point(12, 152);
            this.OptimaSOATextBox.Name = "OptimaSOATextBox";
            this.OptimaSOATextBox.Size = new System.Drawing.Size(233, 23);
            this.OptimaSOATextBox.TabIndex = 2;
            // 
            // DestTextBox
            // 
            this.DestTextBox.Location = new System.Drawing.Point(12, 108);
            this.DestTextBox.Name = "DestTextBox";
            this.DestTextBox.Size = new System.Drawing.Size(233, 23);
            this.DestTextBox.TabIndex = 1;
            // 
            // SourcePathTextBox
            // 
            this.SourcePathTextBox.Location = new System.Drawing.Point(12, 64);
            this.SourcePathTextBox.Name = "SourcePathTextBox";
            this.SourcePathTextBox.Size = new System.Drawing.Size(233, 23);
            this.SourcePathTextBox.TabIndex = 0;
            // 
            // HelpTab
            // 
            this.HelpTab.Location = new System.Drawing.Point(4, 24);
            this.HelpTab.Name = "HelpTab";
            this.HelpTab.Size = new System.Drawing.Size(791, 460);
            this.HelpTab.TabIndex = 2;
            this.HelpTab.Text = "Pomoc";
            this.HelpTab.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 499);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "OptimaSync";
            this.tabControl1.ResumeLayout(false);
            this.SyncTab.ResumeLayout(false);
            this.SyncTab.PerformLayout();
            this.SettingsTab.ResumeLayout(false);
            this.SettingsTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SyncTab;
        private System.Windows.Forms.TabPage SettingsTab;
        private System.Windows.Forms.Button downloadBuildButton;
        private System.Windows.Forms.TabPage HelpTab;
        private System.Windows.Forms.CheckBox SOACheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OptimaSOATextBox;
        private System.Windows.Forms.TextBox DestTextBox;
        private System.Windows.Forms.TextBox SourcePathTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOptimaSOADirectory;
        private System.Windows.Forms.Button buttonDestinationDirectory;
        private System.Windows.Forms.Button buttonSourceDirectory;
        private System.Windows.Forms.Button saveSettingsButton;
    }
}

