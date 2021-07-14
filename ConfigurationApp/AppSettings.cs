using System;
using System.Windows.Forms;
using OptimaSync.Constant;
using Serilog;

namespace OptimaSync.ConfigurationApp
{
    public class AppSettings
    {
        public void SaveSettings(string SourcePath, string DestPath, string OptimaSOAPath, bool isProgrammer)
        {
            try
            {
                Properties.Settings.Default.BuildSourcePath = SourcePath;
                Properties.Settings.Default.BuildDestPath = DestPath;
                Properties.Settings.Default.BuildSOAPath = OptimaSOAPath;
                Properties.Settings.Default.IsProgrammer = isProgrammer;

                Properties.Settings.Default.Save();
                Log.Information(Messages.SETTINGS_SAVED + "\n Build Path: " + SourcePath + "\n Dest Path: " + DestPath + "\n SOA Path: " + OptimaSOAPath + "\n Is Programmer: " + isProgrammer.ToString());
                MessageBox.Show(Messages.SETTINGS_SAVED, Messages.INFORMATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "\n Build Path: " + SourcePath + "\n Dest Path: " + DestPath + "\n SOA Path: " + OptimaSOAPath + "\n Is Programmer: " + isProgrammer.ToString());
            }
        }
    }
}
