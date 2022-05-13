using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Serilog.Events;

namespace OptimaSync.Service
{
    public class SearchOptimaBuildService
    {
        static string[] EXCLUDED_STRINGS = { "CIV", "SQL", "test", "rar", "FIXES", "sPrint" };

        public SearchOptimaBuildService()
        {
        }
        public static DirectoryInfo FindLastOptimaBuild()
        {
            try
            {
                SyncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(AppConfigHelper.GetConfigValue("CompilationPath"));
                var lastBuild = directory.GetDirectories()
                    .Where(q => EXCLUDED_STRINGS.All(c => !q.Name.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
                    .Where(q => q.GetFiles("Common.dll").Length == 1)
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastBuild;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                SyncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.notificationType.Error));
                return null;
            }
        }

        public static void SetLastDownloadedVersion(DirectoryInfo lastDownloadedBuild)
        {
            string lastDownloadedBuildCommonDllPath = lastDownloadedBuild.ToString() + Path.DirectorySeparatorChar + DownloadServiceHelper.CHECK_VERSION_FILE;
            FileVersionInfo lastDownloadedBuildVersionFile = FileVersionInfo.GetVersionInfo(lastDownloadedBuildCommonDllPath);
            string lastDownloadedBuildCommonDllVersion = lastDownloadedBuildVersionFile.ProductVersion.ToString();
            AppConfigHelper.SetConfigValue("LatestVersionChecked", lastDownloadedBuildCommonDllVersion);
        }
        public void AutoCheckNewVersion()
        {
            if (!Convert.ToBoolean(AppConfigHelper.GetConfigValue("AutoCheckVersion")))
            {
                return;
            }

            var lastBuild = FindLastOptimaBuild();

            if (lastBuild == null)
            {
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }
            string lastBuildCommonDllPath = lastBuild.ToString() + Path.DirectorySeparatorChar + DownloadServiceHelper.CHECK_VERSION_FILE;
            FileVersionInfo lastBuildVersionFile = FileVersionInfo.GetVersionInfo(lastBuildCommonDllPath);
            string lastBuildCommonDllVersion = lastBuildVersionFile.ProductVersion.ToString();

            if (lastBuildCommonDllVersion == AppConfigHelper.GetConfigValue("LatestVersionChecked"))
            {
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }

            var myCurrentVersions = GetLatestDownloadedVersion();

            if (myCurrentVersions.Any(x => !lastBuildCommonDllVersion.Contains(x)))
            {
               SyncUI.Invoke(() => MainForm.Notification("Nowa wersja: " + lastBuildCommonDllVersion, NotificationForm.notificationType.Informaton));
                Logger.Write( LogEventLevel.Information,"Nowa wersja: " + lastBuildCommonDllVersion);
                AppConfigHelper.SetConfigValue("LatestVersionChecked", lastBuildCommonDllVersion);
            }
            SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
        }

        private static List<string> GetLatestDownloadedVersion()
        {
            List<string> DownloadedLatestVersions = new List<string>();

            DownloadedLatestVersions.Add(SearchBuildServiceHelper.GetProgrammerVersion());
            DownloadedLatestVersions.Add(SearchBuildServiceHelper.GetSoaVersion());
            DownloadedLatestVersions.Add(SearchBuildServiceHelper.GetBasicVersion());

            return DownloadedLatestVersions;
        }
    }
}
