using OptimaSync.Common;
using OptimaSync.Constant;
using OptimaSync.Service;
using OptimaSync.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Serilog.Events;

namespace OptimaSync.Helper
{
    public class BuildSyncServiceHelper
    {
        public static readonly string LOCK_FILE = "osync.lock";
        public static readonly string CHECK_VERSION_FILE = "Common.dll";

        ValidatorUI validatorUI;
        WindowsService windowsService;
        SyncUI syncUI;

        public BuildSyncServiceHelper(ValidatorUI validatorUI, WindowsService windowsService, SyncUI syncUI)
        {
            this.validatorUI = validatorUI;
            this.windowsService = windowsService;
            this.syncUI = syncUI;
        }

        public bool BuildVersionsAreSame(string buildPath, string buildDirectoryName)
        {
            List<string> buildVersions = new List<string>();

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.PROGRAMMER.ToString())
            {
                string destProgrammerDll = AppConfigHelper.GetConfigValue("ProgrammerDestination") + "\\" + CHECK_VERSION_FILE;

                if (!File.Exists(destProgrammerDll))
                {
                    return false;
                }

                FileVersionInfo dll = FileVersionInfo.GetVersionInfo(destProgrammerDll);
                string dllVersion = dll.ProductVersion.ToString();
                buildVersions.Add(dllVersion);
            }
            else
            {
                List<string> versionList = new List<string>();

                string destSoaDll = AppConfigHelper.GetConfigValue("SOADestination") + "\\" + CHECK_VERSION_FILE;
                string destBuildDll = AppConfigHelper.GetConfigValue("Destination") + "\\" + buildDirectoryName + "\\" + CHECK_VERSION_FILE;

                if (File.Exists(destSoaDll) && AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.SOA.ToString())
                {
                    if (!string.IsNullOrEmpty(AppConfigHelper.GetConfigValue("SOADestination")))
                    {
                        versionList.Add(destSoaDll);
                    }
                    else
                    {
                        return false;
                    }
                }

                if (File.Exists(destBuildDll) && AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString())
                {
                    if (!string.IsNullOrEmpty(AppConfigHelper.GetConfigValue("Destination")))
                    {
                        versionList.Add(destBuildDll);
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (var version in versionList)
                {
                    FileVersionInfo dll = FileVersionInfo.GetVersionInfo(version);
                    string dllVersion = dll.ProductVersion.ToString();
                    buildVersions.Add(dllVersion);
                }
            }
            FileVersionInfo latestVersionDll = FileVersionInfo.GetVersionInfo(buildPath + "\\" + CHECK_VERSION_FILE);
            string latestVersion = latestVersionDll.ProductVersion.ToString();

            if (buildVersions.Any(v => v.Contains(latestVersion)))
            {
                return true;
            }
            return false;
        }

        public string ChooseExtractionPath(DirectoryInfo dir)
        {
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.SOA.ToString() &&
                !SOARequirementsAreMet())
            {
                return null;
            }

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadTypeEnum.BASIC.ToString() &&
                !validatorUI.DestPathIsValid())
            {
                return null;
            }

            switch (AppConfigHelper.GetConfigValue("DownloadType"))
            {
                case "PROGRAMMER":
                    return AppConfigHelper.GetConfigValue("ProgrammerDestination");
                case "SOA":
                    return AppConfigHelper.GetConfigValue("SOADestination");
                case "BASIC":
                    return AppConfigHelper.GetConfigValue("Destination") + "\\" + dir.Name;
                default:
                    syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return null;
            }
        }

        public bool DoesLockFileExist(string path)
        {
            if (File.Exists(path + "\\" + LOCK_FILE))
            {
                return true;
            }
            return false;
        }

        public void RunOptima(string path)
        {
            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima")))
            {
                Process.Start(path + "\\" + "Comarch OPT!MA.exe");
            }
        }

        public void CreateLockFile(string extractionPath)
        {
            File.Create(extractionPath + "\\" + LOCK_FILE).Close();
        }

        public void DeleteLockFile(string lockFilePath)
        {
            File.Delete(lockFilePath + "\\" + LOCK_FILE);
        }

        private bool SOARequirementsAreMet()
        {
            if (!validatorUI.DestSOAPathIsValid())
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }

            if (!windowsService.DoesSOAServiceExist())
            {
                Logger.Write(LogEventLevel.Error, Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!windowsService.SoaIsStopped())
            {
                Logger.Write(LogEventLevel.Error, Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
