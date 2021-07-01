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
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("configuration/appSettings"); // TODO Refresh do not work in runtime. Need to restart app...
        }

        public string GetSourcePath()
        {
            return config.AppSettings.Settings["SourcePath"].Value;
        }

        public string GetDestPath()
        {
            return config.AppSettings.Settings["DestPath"].Value;
        }

        public string GetOptimaSOAPath()
        {
            return config.AppSettings.Settings["OptimaSOAPath"].Value;
        }
    }
}
