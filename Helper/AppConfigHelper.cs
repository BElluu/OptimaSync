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
            ConfigurationManager.AppSettings.Set(key, value);
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        static string GET_DOWNLOAD_TYPE = ConfigurationManager.AppSettings.Get("DownloadType");
        static bool GET_RUN_OPTIMA = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RunOptima"));
        static string GET_LATEST_VERSION_CHECKED = ConfigurationManager.AppSettings.Get("LatestVersionChecked");
        static string GET_COMPILATION_PATH = ConfigurationManager.AppSettings.Get("CompilationPath");
        static string GET_DESTINATION = ConfigurationManager.AppSettings.Get("Destination");
        static string GET_SOA_DESTINATION = ConfigurationManager.AppSettings.Get("SOADestination");
        static string GET_PROGRAMMER_DESTINATION = ConfigurationManager.AppSettings.Get("ProgrammerDestination");
        static bool GET_AUTO_CHECK_VERSION = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AutoCheckVersion"));
        static bool GET_NOTIFICATION_SOUND = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("NotificationSound"));
    }
}
