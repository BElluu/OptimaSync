using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptimaSync.UI
{
    public class SyncUI
    {
        public void PathToTextbox(TextBox textBox)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox.AppendText(folderBrowserDialog.SelectedPath);
        }
    }
}
