using System;
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
            }catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
