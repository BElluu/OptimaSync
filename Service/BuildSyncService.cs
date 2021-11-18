using System;
using System.IO;
using OptimaSync.Constant;
using OptimaSync.UI;
using OptimaSync.Helper;
using OptimaSync.Common;
using Serilog.Events;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        SyncUI syncUI;
        RegisterOptimaService registerDLL;
        BuildSyncServiceHelper buildSyncHelper;
        SearchBuildService searchBuild;

        public BuildSyncService(SyncUI syncUI, RegisterOptimaService registerDLL, BuildSyncServiceHelper buildSyncHelper, SearchBuildService searchBuild)
        {
            this.syncUI = syncUI;
            this.registerDLL = registerDLL;
            this.buildSyncHelper = buildSyncHelper;
            this.searchBuild = searchBuild;
        }

        public void GetOptimaBuild()
        {
            syncUI.EnableElementsOnForm(false);

            string buildServer = AppConfigHelper.GetConfigValue("BuildServer");
            if (!NetworkDrive.HaveAccessToHost(buildServer))
            {
                SyncUI.Invoke(() => MainForm.Notification("Brak dostępu do " + buildServer, NotificationForm.enumType.Error));
                Logger.Write(LogEventLevel.Error, "Brak dostępu do " + buildServer + "! Sprawdź czy masz internet lub połączenie VPN.");
                return;
            }

            try
            {
                var lastBuildDir = searchBuild.FindLastBuild();
                string extractionPath = buildSyncHelper.ChooseExtractionPath(lastBuildDir);

                if (lastBuildDir == null || string.IsNullOrEmpty(extractionPath) || haveLatestVersion(lastBuildDir, extractionPath))
                {
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return;
                }

                DownloadBuild(lastBuildDir, extractionPath);
                registerDLL.RegisterOptima(extractionPath);
            }
            catch
            {
                syncUI.EnableElementsOnForm(true);
            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }

        public bool DownloadBuild(DirectoryInfo lastBuildDir, string extractionPath)
        {
            var files = filesToCopy(lastBuildDir);

            try
            {
                if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                    !Directory.Exists(extractionPath))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath);
                }

                if (!buildSyncHelper.DoesLockFileExist(extractionPath))
                {
                    buildSyncHelper.CreateLockFile(extractionPath);
                }

                syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                foreach (string dir in Directory.GetDirectories(lastBuildDir.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(lastBuildDir.ToString(), extractionPath));
                }

                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(lastBuildDir.ToString(), extractionPath), true);
                    syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + lastBuildDir.Name);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return false;
            }
        }

        private string[] filesToCopy(DirectoryInfo lastBuildDir)
        {
            return Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
        }

        private bool haveLatestVersion(DirectoryInfo lastBuildDir, string extractionPath)
        {
            if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                buildSyncHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                Logger.Write(LogEventLevel.Information, Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return true;
            }
            return false;
        }
    }
}
