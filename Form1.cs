using OptimaSync.Compilation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.ConfigurationApp;
using System.Configuration;
using System.Reflection;

namespace OptimaSync
{
    public partial class Form1 : Form
    {
        CompilationSync compilationSync = new CompilationSync();
        SyncUI syncUI = new SyncUI();
        Settings settings = new Settings();
        Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        public Form1()
        {
            InitializeComponent();
            this.SourcePathTextBox.Text = config.AppSettings.Settings["SourcePath"].Value;
            this.DestTextBox.Text = config.AppSettings.Settings["DestPath"].Value;
            this.OptimaSOATextBox.Text = config.AppSettings.Settings["OptimaSOAPath"].Value;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void downloadBuildButton_Click(object sender, EventArgs e)
        {
            if (SOACheckBox.Checked && string.IsNullOrEmpty(OptimaSOATextBox.Text))
            {
                MessageBox.Show("Chcesz wykorzystać SOA. Ścieżka instalacyjna Optimy musi być uzupełniona!");
            }
            else
            {
                compilationSync.DownloadLatestCompilation();
            }
        }

        private void SyncTab_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

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
                MessageBox.Show("Ścieżka kompilacji nie może być pusta!");
            }
            else
            {
                settings.SetPaths(SourcePathTextBox.Text, DestTextBox.Text, OptimaSOATextBox.Text);
            }
        }
    }
}
