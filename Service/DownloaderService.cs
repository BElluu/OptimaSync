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
    public class DownloaderService
    {
        SyncUI syncUI;
        RegisterOptimaService registerDLL;
        BuildSyncServiceHelper buildSyncHelper;
        SearchBuildService searchBuild;

        public DownloaderService(SyncUI syncUI, RegisterOptimaService registerDLL, BuildSyncServiceHelper buildSyncHelper, SearchBuildService searchBuild)
        {
            this.syncUI = syncUI;
            this.registerDLL = registerDLL;
            this.buildSyncHelper = buildSyncHelper;
            this.searchBuild = searchBuild;
        }

        public void GetOptima(bool buildVersion, string prodVersionPath, bool shouldDownloadEDeclaration)
        {
            string server = string.Empty;
            string extractionPath = string.Empty;
            DirectoryInfo versionToDownload = null;
            try
            {
                syncUI.EnableElementsOnForm(false);

                if (buildVersion)
                {
                    /*server = AppConfigHelper.GetConfigValue("BuildServer");
                    versionToDownload = searchBuild.FindLastOptimaBuild();*/
                    server = AppConfigHelper.GetConfigValue("BuildServer");

                    if (!NetworkDrive.HaveAccessToHost(server))
                        return;
                    if (!DataForBuildVersionAreValid(out extractionPath, out versionToDownload))
                        return;
                }
                else if (!buildVersion)
                {
                    /*server = AppConfigHelper.GetConfigValue("ProductionServer");
                    versionToDownload = new DirectoryInfo(prodVersionPath);*/
                    server = AppConfigHelper.GetConfigValue("ProductionServer");
                    if (!NetworkDrive.HaveAccessToHost(server))
                        return;
                    if (!DataForProductionVersionAreValid(out extractionPath, out versionToDownload, prodVersionPath))
                        return;
                }

               // string extractionPath = buildSyncHelper.ChooseExtractionPath(versionToDownload);

                if (DownloadOptima(versionToDownload, extractionPath))
                {
                    if(buildVersion)
                        searchBuild.SetLastDownloadedVersion(versionToDownload);
                    registerDLL.RegisterOptima(extractionPath);
                }

                if(shouldDownloadEDeclaration)
                {
                    DownloadEDeclaration(extractionPath);
                }

            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }

        private void DownloadEDeclaration(string extractionPath)
        {
            var eDeclarationToDownload = searchBuild.FindLastEDeclarationBuild();
            var files = filesToCopy(eDeclarationToDownload);

            try
            {
                if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                    !Directory.Exists(extractionPath + "\\Deklaracje"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(extractionPath + "\\Deklaracje");
                }
            }catch (Exception ex)
            {

            }
        }

/*        public void GetOptimaBuild()
        {
            try
            {
                syncUI.EnableElementsOnForm(false);

                string buildServer = AppConfigHelper.GetConfigValue("BuildServer");
                if (!NetworkDrive.HaveAccessToHost(buildServer))
                {
                    SyncUI.Invoke(() => MainForm.Notification("Brak dostępu do " + buildServer, NotificationForm.enumType.Error));
                    Logger.Write(LogEventLevel.Error, "Brak dostępu do " + buildServer + "! Sprawdź czy masz internet lub połączenie VPN.");
                    return;
                }

                var lastBuildDir = searchBuild.FindLastOptimaBuild();
                string extractionPath = buildSyncHelper.ChooseExtractionPath(lastBuildDir);

                if (lastBuildDir == null || string.IsNullOrEmpty(extractionPath) || haveLatestVersion(lastBuildDir, extractionPath))
                {
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return;
                }
                if (DownloadOptima(lastBuildDir, extractionPath))
                {
                    searchBuild.SetLastDownloadedVersion(lastBuildDir);
                    registerDLL.RegisterOptima(extractionPath);
                }
            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }*/

        private bool DownloadOptima(DirectoryInfo versionToDownload, string extractionPath)
        {
            var files = filesToCopy(versionToDownload);

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
                foreach (string dir in Directory.GetDirectories(versionToDownload.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(versionToDownload.ToString(), extractionPath));
                }

                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(versionToDownload.ToString(), extractionPath), true);
                    syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + versionToDownload.Name);
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

/*        public void GetOptimaProduction(DirectoryInfo prodVersion)
        {
            try
            {
                syncUI.EnableElementsOnForm(false);

                string productionServer = AppConfigHelper.GetConfigValue("ProductionServer");
                if (!NetworkDrive.HaveAccessToHost(productionServer))
                {
                    SyncUI.Invoke(() => MainForm.Notification("Brak dostępu do " + productionServer, NotificationForm.enumType.Error));
                    Logger.Write(LogEventLevel.Error, "Brak dostępu do " + productionServer + "! Sprawdź czy masz internet lub połączenie VPN.");
                    return;
                }

                string extractionPath = buildSyncHelper.ChooseExtractionPath(prodVersion);

                if (string.IsNullOrEmpty(extractionPath))
                {
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return;
                }
                if (DownloadProduction(prodVersion, extractionPath))
                {
                    registerDLL.RegisterOptima(extractionPath);
                }
            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }
        public bool DownloadProduction(DirectoryInfo prodDir, string extractionPath)
        {
            var files = filesToCopy(prodDir);

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
                foreach (string dir in Directory.GetDirectories(prodDir.ToString(), "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dir.Replace(prodDir.ToString(), extractionPath));
                }

                syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", 0, files.Length));
                int i = 0;

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(prodDir.ToString(), extractionPath), true);
                    syncUI.ChangeProgressLabel(string.Format(Messages.DOWNLOADING_BUILD + " {0}/{1}", ++i, files.Length));
                }

                Logger.Write(LogEventLevel.Information, "Skopiowano " + prodDir.Name);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(LogEventLevel.Error, ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                SyncUI.Invoke(() => MainForm.Notification(Messages.ERROR_CHECK_LOGS, NotificationForm.enumType.Error));
                return false;
            }
        }*/

        public Dictionary<string, string> GetListOfProd()
        {
            var listOfBuilds = new Dictionary<string, string>();

            var directory = new DirectoryInfo(AppConfigHelper.GetConfigValue("ProductionPath"));
            var prodVersions = directory.GetDirectories();

            foreach (DirectoryInfo prod in prodVersions)
            {
                listOfBuilds.Add(prod.Name, prod.ToString());
            };

            return listOfBuilds;
        }

        private bool DataForBuildVersionAreValid(out string extractionPath, out DirectoryInfo versionToDownload)
        {
            versionToDownload = searchBuild.FindLastOptimaBuild();
            extractionPath = buildSyncHelper.ChooseExtractionPath(versionToDownload);

            if (versionToDownload == null || string.IsNullOrEmpty(extractionPath) || haveLatestVersion(versionToDownload, extractionPath))
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }
            return true;
        }

        private bool DataForProductionVersionAreValid(out string extractionPath, out DirectoryInfo versionToDownload, string prodVersionPath)
        {
            versionToDownload = new DirectoryInfo(prodVersionPath); ;
            extractionPath = buildSyncHelper.ChooseExtractionPath(versionToDownload);

            if (versionToDownload == null || string.IsNullOrEmpty(extractionPath))
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }
            return true;
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
