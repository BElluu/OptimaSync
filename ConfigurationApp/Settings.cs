using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OptimaSync.ConfigurationApp
{
    public class Settings
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
        public void SetPaths(string SourcePath, string DestPath, string OptimaSOAPath)
        {
            config.AppSettings.Settings["SourcePath"].Value = SourcePath;
            config.AppSettings.Settings["DestPath"].Value = DestPath;
            config.AppSettings.Settings["OptimaSOAPath"].Value = OptimaSOAPath;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
