using System;
using OptimaSync.Constant;
using Serilog;

namespace OptimaSync.ConfigurationApp
{
    public class AppSettings
    {
        public void SetPaths(string SourcePath, string DestPath, string OptimaSOAPath)
        {
            try
            {
                Properties.Settings.Default.BuildSourcePath = SourcePath;
                Properties.Settings.Default.BuildDestPath = DestPath;
                Properties.Settings.Default.BuildSOAPath = OptimaSOAPath;

                Properties.Settings.Default.Save();
                Log.Information(Messages.PATHS_SAVED + "\n Build Path: " + SourcePath + "\n Dest Path: " + DestPath + "\n SOA Path: " + OptimaSOAPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "\n Build Path: " + SourcePath + "\n Dest Path: " + DestPath + "\n SOA Path: " + OptimaSOAPath);
            }
        }
    }
}
