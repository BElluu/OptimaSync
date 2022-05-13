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
    public class DownloadServiceHelper
    {
        public static readonly string LOCK_FILE = "osync.lock";
        public static readonly string CHECK_VERSION_FILE = "Common.dll";

        public DownloadServiceHelper() { }

        public static bool BuildVersionsAreSame(string buildPath, string buildDirectoryName)
        {
            List<string> buildVersions = new List<string>();

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.PROGRAMMER.ToString())
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

                if (File.Exists(destSoaDll) &&
                    AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.SOA.ToString() &&
                    !string.IsNullOrEmpty(AppConfigHelper.GetConfigValue("SOADestination")))
                {
                   versionList.Add(destSoaDll);
                }
                else { return false; }

                if (File.Exists(destBuildDll) &&
                    AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.BASIC.ToString() &&
                    !string.IsNullOrEmpty(AppConfigHelper.GetConfigValue("Destination")))
                {
                   versionList.Add(destBuildDll);
                }
                else { return false; }

                foreach (var version in versionList)
                {
                    FileVersionInfo dll = FileVersionInfo.GetVersionInfo(version);
                    string dllVersion = dll.ProductVersion.ToString();
                    buildVersions.Add(dllVersion);
                }
            }
            return BuildVersionIsSameAsOwned(buildPath,buildVersions);
        }

        public static string ChooseExtractionPath(DirectoryInfo dir)
        {
            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.SOA.ToString() &&
                !SOARequirementsAreMet())
            {
                return null;
            }

            if (AppConfigHelper.GetConfigValue("DownloadType") == DownloadType.BASIC.ToString() &&
                !ValidatorUI.DestPathIsValid())
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
                    return AppConfigHelper.GetConfigValue("Destination") + Path.DirectorySeparatorChar + dir.Name;
                default:
                    SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                    return null;
            }
        }

        public static bool DoesLockFileExist(string path)
        {
            if (File.Exists(path + Path.DirectorySeparatorChar + LOCK_FILE))
            {
                return true;
            }
            return false;
        }

        public static void RunOptima(string path)
        {
            if (Convert.ToBoolean(AppConfigHelper.GetConfigValue("RunOptima")))
            {
                Process.Start(path + Path.DirectorySeparatorChar + "Comarch OPT!MA.exe");
            }
        }

        public static void CreateLockFile(string extractionPath)
        {
            File.Create(extractionPath + Path.DirectorySeparatorChar + LOCK_FILE).Close();
        }

        public static void DeleteLockFile(string lockFilePath)
        {
            File.Delete(lockFilePath + Path.DirectorySeparatorChar + LOCK_FILE);
        }

        public static string[] filesToCopy(DirectoryInfo lastBuildDir)
        {
            return Directory.GetFiles(lastBuildDir.ToString(), "*.*", SearchOption.AllDirectories);
        }

        private static bool SOARequirementsAreMet()
        {
            if (!ValidatorUI.DestSOAPathIsValid())
            {
                SyncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }

            if (!WindowsService.DoesSOAServiceExist())
            {
                Logger.Write(LogEventLevel.Error, Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!WindowsService.SoaIsStopped())
            {
                Logger.Write(LogEventLevel.Error, Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool BuildVersionIsSameAsOwned(string buildPath, List<string> buildVersions)
        {
            FileVersionInfo latestVersionDll = FileVersionInfo.GetVersionInfo(buildPath + Path.DirectorySeparatorChar + CHECK_VERSION_FILE);
            string latestVersion = latestVersionDll.ProductVersion.ToString();

            if (buildVersions.Any(v => v.Contains(latestVersion)))
            {
                return true;
            }
            return false;
        }
    }
}
