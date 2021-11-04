using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OptimaSync.Helper
{
   public static class AppConfigHelper
    {
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static void SetConfigValue(string key, string value)
        {
            Configuration configuration = ConfigurationManager.
            OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.AppSettings.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
