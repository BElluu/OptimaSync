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
    internal class SearchBuildService
    {
        static string[] EXCLUDED_STRINGS = { "CIV", "SQL", "test", "rar", "FIXES" };
        SyncUI syncUI = new SyncUI();
        SearchBuildServiceHelper searchBuildServiceHelper = new SearchBuildServiceHelper();
        internal DirectoryInfo FindLastBuild()
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
                return null;
            }
        }
        public void AutoCheckNewVersion()
        {
            var lastBuild = FindLastBuild();

            if (lastBuild == null)
            {
                return;
            }
            string lastBuildCommonDllPath = lastBuild.ToString() + '\\' + BuildSyncServiceHelper.CHECK_VERSION_FILE;
            FileVersionInfo lastBuildVersionFile = FileVersionInfo.GetVersionInfo(lastBuildCommonDllPath);
            string lastBuildCommonDllVersion = lastBuildVersionFile.ProductVersion.ToString();

            if (lastBuildCommonDllVersion == Properties.Settings.Default.LatestCheckedVersion)
            {
                return;
            }

            var myCurrentVersions = GetLatestDownloadedVersion();

            if (myCurrentVersions.Any(x => !lastBuildCommonDllVersion.Contains(x)))
            {
                MainForm.Notification("Nowa wersja: " + lastBuildCommonDllVersion, NotificationForm.enumType.Informaton);
                Properties.Settings.Default.LatestCheckedVersion = lastBuildCommonDllVersion;
                Properties.Settings.Default.Save();
            }
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
