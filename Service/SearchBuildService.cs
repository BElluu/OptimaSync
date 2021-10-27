using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Helper;
using OptimaSync.UI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OptimaSync.Service
{
    public class SearchBuildService
    {
        static string[] EXCLUDED_STRINGS = { "CIV", "SQL", "test", "rar", "FIXES" };
        SyncUI syncUI = new SyncUI();
        SearchBuildServiceHelper searchBuildServiceHelper = new SearchBuildServiceHelper();
        public DirectoryInfo FindLastBuild()
        {
            if (!NetworkDrive.HaveAccessToHost("natalie"))
            {
                SyncUI.Invoke(() => MainForm.Notification("Brak dostępu do natalie", NotificationForm.enumType.Error));
                Log.Error(Messages.ACCESS_TO_HOST_ERROR);
                return null;
            }

            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(Properties.Settings.Default.BuildSourcePath);
                var lastBuild = directory.GetDirectories()
                    .Where(q => EXCLUDED_STRINGS.All(c => !q.Name.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastBuild;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return null;
            }
        }
        public void AutoCheckNewVersion()
        {
            if (Properties.Settings.Default.NewVersionNotifications == false)
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

            if (lastBuildCommonDllVersion == Properties.Settings.Default.LatestCheckedVersion)
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return;
            }

            //TODO Add latest version after try download version.

            var myCurrentVersions = GetLatestDownloadedVersion();

            if (myCurrentVersions.Any(x => !lastBuildCommonDllVersion.Contains(x)))
            {
               SyncUI.Invoke(() => MainForm.Notification("Nowa wersja: " + lastBuildCommonDllVersion, NotificationForm.enumType.Informaton));
                Log.Information("Nowa wersja: " + lastBuildCommonDllVersion);
                Properties.Settings.Default.LatestCheckedVersion = lastBuildCommonDllVersion;
                Properties.Settings.Default.Save();
            }
            syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
        }

        private List<string> GetLatestDownloadedVersion()
        {
            List<string> DownloadedLatestVersions = new List<string>();

            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetProgrammerVersion());
            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetSoaVersion());
            DownloadedLatestVersions.Add(searchBuildServiceHelper.GetLastBuildVersion());

            return DownloadedLatestVersions;
        }
    }
}
