using System;
using System.IO;
using OptimaSync.Constant;
using Serilog;
using OptimaSync.UI;
using OptimaSync.Helper;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        SyncUI syncUI;
        RegisterDLLService registerDLL;
        BuildSyncServiceHelper buildSyncHelper;
        SearchBuildService searchBuild;

        public BuildSyncService(SyncUI syncUI, RegisterDLLService registerDLL, BuildSyncServiceHelper buildSyncHelper, SearchBuildService searchBuild)
        {
            this.syncUI = syncUI;
            this.registerDLL = registerDLL;
            this.buildSyncHelper = buildSyncHelper;
            this.searchBuild = searchBuild;
        }

        public void PrepareOptimaBuild(DownloadTypeEnum type)
        {
            syncUI.EnableElementsOnForm(false);
            registerDLL.RegisterOptima(DownloadBuild(type), type);
            syncUI.EnableElementsOnForm(true);
        }

        public string DownloadBuild(DownloadTypeEnum type)
        {
            var dir = searchBuild.FindLastBuild();
            if (dir == null)
            {
                return null;
            }

            string extractionPath = buildSyncHelper.ChooseExtractionPath(type, dir);

            if (extractionPath == null)
            {
                return null;
            }

            if (!buildSyncHelper.DoesLockFileExist(extractionPath) &&
                buildSyncHelper.BuildVersionsAreSame(dir.ToString(), type, dir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                Log.Information(Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return null;
            }

            try
            {
                if ((type == DownloadTypeEnum.BASIC || type == DownloadTypeEnum.SOA) && 
                    !Directory.Exists(extractionPath))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath);
                }

                if (!buildSyncHelper.DoesLockFileExist(extractionPath))
                {
                    buildSyncHelper.CreateLockFile(extractionPath);
                }

                syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                foreach (string dirPath in Directory.GetDirectories(dir.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(dir.ToString(), extractionPath));
                }

                string[] filesToCopy = Directory.GetFiles(dir.ToString(), "*.*", SearchOption.AllDirectories);
                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, filesToCopy.Length));
                int i = 0;

                foreach (string newPath in filesToCopy)
                {
                    File.Copy(newPath, newPath.Replace(dir.ToString(), extractionPath), true);
                    syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, filesToCopy.Length));
                }

                Log.Information("Skopiowano " + dir.Name);
                return extractionPath;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return null;
            }
        }
    }
}
