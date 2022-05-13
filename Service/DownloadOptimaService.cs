using System;
using System.IO;
using OptimaSync.Constant;
using OptimaSync.UI;
using OptimaSync.Helper;
using OptimaSync.Common;
using Serilog.Events;
using System.Collections.Generic;

namespace OptimaSync.Service
{
    public class DownloadOptimaService
    {
        private readonly RegisterOptimaService registerDLL;
        private readonly DownloadServiceHelper buildSyncHelper;
        private readonly DownloadEDeclarationService downloadEDeclaration;

        public DownloadOptimaService(RegisterOptimaService registerDLL, DownloadServiceHelper buildSyncHelper, DownloadEDeclarationService downloadEDeclaration)
        {
            this.registerDLL = registerDLL;
            this.buildSyncHelper = buildSyncHelper;
            this.downloadEDeclaration = downloadEDeclaration;
        }

        public void GetOptima(bool buildVersion, string prodVersionPath, bool shouldDownloadEDeclaration)
        {
            string server = string.Empty;
            string extractionPath = string.Empty;
            DirectoryInfo versionToDownload = null;
            try
            {
                SyncUI.EnableElementsOnForm(false);

                if (buildVersion)
                {
                    server = AppConfigHelper.GetConfigValue("BuildServer");

                    if (!NetworkDrive.HaveAccessToHost(server))
                        return;
                    if (!DataForBuildVersionAreValid(out extractionPath, out versionToDownload))
                        return;
                }
                else
                {
                    server = AppConfigHelper.GetConfigValue("ProductionServer");
                    if (!NetworkDrive.HaveAccessToHost(server))
                        return;
                    if (!DataForProductionVersionAreValid(out extractionPath, out versionToDownload, prodVersionPath))
                        return;
                }

                if (DownloadOptima(versionToDownload, extractionPath))
                {
                    if(buildVersion)
                        SearchOptimaBuildService.SetLastDownloadedVersion(versionToDownload);
                    registerDLL.RegisterOptima(extractionPath);
                }

                if(shouldDownloadEDeclaration)
                {
                    downloadEDeclaration.DownloadEDeclaration(extractionPath);
                }

            }
            finally
            {
                SyncUI.EnableElementsOnForm(true);
            }
        }

        private static bool DownloadOptima(DirectoryInfo versionToDownload, string extractionPath)
        {
            var files = filesToCopy(versionToDownload);

            try
            {
                if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.BASIC.ToString() &&
                    !Directory.Exists(extractionPath))
                {
                    Directory.CreateDirectory(extractionPath);
                }

                if (!DownloadServiceHelper.DoesLockFileExist(extractionPath))
                {
                    DownloadServiceHelper.CreateLockFile(extractionPath);
                }

                SyncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                foreach (string dir in Directory.GetDirectories(versionToDownload.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(versionToDownload.ToString(), extractionPath));
                }

                SyncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(versionToDownload.ToString(), extractionPath), true);
                    SyncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + versionToDownload.Name);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                SyncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.notificationType.Error));
                return false;
            }
        }

        public static Dictionary<string, string> GetListOfProd()
        {
            var listOfBuilds = new Dictionary<string, string>();
            DirectoryInfo directory;
            DirectoryInfo[] prodVersions;
            try
            {
                directory = new DirectoryInfo(AppConfigHelper.GetConfigValue("ProductionPath"));
                prodVersions = directory.GetDirectories();

            }
            catch(Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                Logger.Write(LogEventLevel.Error, "Nie mozna zaladowac listy wersji produkcyjnych");
                listOfBuilds.Add("", "");
                return listOfBuilds;
            }
            
            prodVersions = directory.GetDirectories();

            foreach (DirectoryInfo prod in prodVersions)
            {
                listOfBuilds.Add(prod.Name, prod.ToString());
            }

            return listOfBuilds;
        }

        private bool DataForBuildVersionAreValid(out string extractionPath, out DirectoryInfo versionToDownload)
        {
            versionToDownload = SearchOptimaBuildService.FindLastOptimaBuild();
            extractionPath = buildSyncHelper.ChooseExtractionPath(versionToDownload);

            if (versionToDownload == null || string.IsNullOrEmpty(extractionPath) || haveLatestVersion(versionToDownload, extractionPath))
            {
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }
            return true;
        }

        private bool DataForProductionVersionAreValid(out string extractionPath, out DirectoryInfo versionToDownload, string prodVersionPath)
        {
            versionToDownload = new DirectoryInfo(prodVersionPath);
            extractionPath = buildSyncHelper.ChooseExtractionPath(versionToDownload);

            if (!versionToDownload.Exists || string.IsNullOrEmpty(extractionPath))
            {
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }
            return true;
        }

        private static string[] filesToCopy(DirectoryInfo lastBuildDir)
        {
            return Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
        }

        private static bool haveLatestVersion(DirectoryInfo lastBuildDir, string extractionPath)
        {
            if (!DownloadServiceHelper.DoesLockFileExist(extractionPath) &&
                DownloadServiceHelper.BuildVersionsAreSame(lastBuildDir.ToString(), lastBuildDir.Name))
            {
                SyncUI.Invoke(() => MainForm.Notification(Messages.YOU_HAVE_LATEST_BUILD, NotificationForm.notificationType.Informaton));
                Logger.Write(LogEventLevel.Information, Messages.YOU_HAVE_LATEST_BUILD);
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return true;
            }
            return false;
        }
    }
}
