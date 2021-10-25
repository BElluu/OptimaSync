﻿using System;
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

        public void PrepareOptimaBuild(bool withSoa, bool isProgrammer)
        {
            syncUI.EnableElementsOnForm(false);
            registerDLL.RegisterOptima(DownloadBuild(isProgrammer, withSoa), isProgrammer);
            syncUI.EnableElementsOnForm(true);
        }

        public string DownloadBuild(bool isProgrammer, bool withSOA)
        {
            var dir = searchBuild.FindLastBuild();
            if (dir == null)
            {
                return null;
            }

            string extractionPath = buildSyncHelper.ChooseExtractionPath(isProgrammer, withSOA, dir);

            if (extractionPath == null)
            {
                return null;
            }

            if (!buildSyncHelper.DoesLockFileExist(extractionPath))
            {
                if (buildSyncHelper.BuildVersionsAreSame(dir.ToString(), isProgrammer, withSOA, dir.Name))
                {
                    SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.enumType.Informaton));
                    Log.Information(Messages.YOU_HAVE_LATEST_BUILD);
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return null;
                }
            }

            try
            {
                if ((!isProgrammer && !withSOA) && !Directory.Exists(extractionPath))
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
