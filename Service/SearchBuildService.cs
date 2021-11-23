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
    public class SearchBuildService
    {
        static string[] EXCLUDED_STRINGS = { "CIV", "SQL", "test", "rar", "FIXES" };
        SyncUI syncUI = new SyncUI();
        SearchBuildServiceHelper searchBuildServiceHelper = new SearchBuildServiceHelper();
        public DirectoryInfo FindLastBuild()
        {
            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(AppConfigHelper.GetConfigValue("CompilationPath"));
                var lastBuild = directory.GetDirectories()
                    .Where(q => EXCLUDED_STRINGS.All(c => !q.Name.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastBuild;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return null;
            }
        }

        public void SetLastDownloadedVersion(DirectoryInfo lastDownloadedBuild)
        {
            string lastDownloadedBuildCommonDllPath = lastDownloadedBuild.ToString() + '\\' + BuildSyncServiceHelper.CHECK_VERSION_FILE;
            FileVersionInfo lastDownloadedBuildVersionFile = FileVersionInfo.GetVersionInfo(lastDownloadedBuildCommonDllPath);
            string lastDownloadedBuildCommonDllVersion = lastDownloadedBuildVersionFile.ProductVersion.ToString();
            AppConfigHelper.SetConfigValue("LatestVersionChecked", lastDownloadedBuildCommonDllVersion);
        }
        public void AutoCheckNewVersion()
        {
            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("AutoCheckVersion")) == false)
            {
                return;
            }

            var lastBuild = FindLastBuild();

            if (lastBuild == null)
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }
            string lastBuildCommonDllPath = lastBuild.ToString() + '\\' + BuildSyncServiceHelper.CHECK_VERSION_FILE;
            FileVersionInfo lastBuildVersionFile = FileVersionInfo.GetVersionInfo(lastBuildCommonDllPath);
            string lastBuildCommonDllVersion = lastBuildVersionFile.ProductVersion.ToString();

            if (lastBuildCommonDllVersion == AppConfigHelper.GetConfigValue("LatestVersionChecked"))
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }

            var myCurrentVersions = GetLatestDownloadedVersion();

            if (myCurrentVersions.Any(x => !lastBuildCommonDllVersion.Contains(x)))
            {
               SyncUI.Invoke(() => MainForm.Notification("Nowa wersja: " + lastBuildCommonDllVersion, NotificationForm.enumType.Informaton));
                Logger.Write( LogEventLevel.Information,"Nowa wersja: " + lastBuildCommonDllVersion);
                AppConfigHelper.SetConfigValue("LatestVersionChecked", lastBuildCommonDllVersion);
            }
            syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
        }

        private List<string> GetLatestDownloadedVersion()
        {
            List<string> DownloadedLatestVersions = new List<string>();

            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetProgrammerVersion());
            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetSoaVersion());
            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetBasicVersion());

            return DownloadedLatestVersions;
        }
    }
}
