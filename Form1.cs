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

namespace OptimaSync
{
    public partial class Form1 : Form
    {
        CompilationSync compilationSync = new CompilationSync();
        SyncUI syncUI = new SyncUI();
        Settings settings = new Settings();
        public Form1()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            compilationSync.DownloadLatestCompilation();
        }

        private void SyncTab_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
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
            settings.SetPaths(SourcePathTextBox.Text, DestTextBox.Text, OptimaSOATextBox.Text);
        }
    }
}
