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
/*        public static string GET_DOWNLOAD_TYPE = ConfigurationManager.AppSettings.Get("DownloadType");
        public static bool GET_RUN_OPTIMA = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RunOptima"));
        public static string GET_LATEST_VERSION_CHECKED = ConfigurationManager.AppSettings.Get("LatestVersionChecked");
        public static string GET_COMPILATION_PATH = ConfigurationManager.AppSettings.Get("CompilationPath");
        public static string GET_DESTINATION = ConfigurationManager.AppSettings.Get("Destination");
        public static string GET_SOA_DESTINATION = ConfigurationManager.AppSettings.Get("SOADestination");
        public static string GET_PROGRAMMER_DESTINATION = ConfigurationManager.AppSettings.Get("ProgrammerDestination");
        public static bool GET_AUTO_CHECK_VERSION = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AutoCheckVersion"));
        public static bool GET_NOTIFICATION_SOUND = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("NotificationSound"));*/

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
