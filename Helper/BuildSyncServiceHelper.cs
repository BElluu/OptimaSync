﻿using OptimaSync.Constant;
using OptimaSync.Service;
using OptimaSync.UI;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        public bool BuildVersionsAreSame(string buildPath, bool isProgrammer, bool withSOA, string buildDirectoryName)
        {
            List<string> buildVersions = new List<string>();
            if (isProgrammer)
            {
                string destProgrammerDll = Properties.Settings.Default.ProgrammersPath + "\\" + CHECK_VERSION_FILE;

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
                string destSoaDll = Properties.Settings.Default.BuildSOAPath + "\\" + CHECK_VERSION_FILE;
                string destBuildDll = Properties.Settings.Default.BuildDestPath + "\\" + buildDirectoryName + "\\" + CHECK_VERSION_FILE;

                if ((!File.Exists(destSoaDll) && withSOA) || (!File.Exists(destBuildDll) && !withSOA))
                {
                    return false;
                }

                List<string> versionList = new List<string>();
                versionList.Add(destSoaDll);
                versionList.Add(destBuildDll);

                foreach(var version in versionList)
                {
                    FileVersionInfo dll = FileVersionInfo.GetVersionInfo(version);
                    string dllVersion = dll.ProductVersion.ToString();
                    buildVersions.Add(dllVersion);
                }
            }
            FileVersionInfo latestVersionDll  = FileVersionInfo.GetVersionInfo(buildPath + "\\" + CHECK_VERSION_FILE);
            string latestVersion = latestVersionDll.ProductVersion.ToString();

            if (buildVersions.Any(v => v.Contains(latestVersion)))
            {
                return true;
            }
            return false;
        }

        public string ChooseExtractionPath(bool isProgrammer, bool withSOA, DirectoryInfo dir)
        {
            if (isProgrammer)
            {
                return Properties.Settings.Default.ProgrammersPath;
            }
            else if (withSOA && SOARequirementsAreMet())
            {
                return Properties.Settings.Default.BuildSOAPath;
            }
            else if (validatorUI.DestPathIsValid())
            {
               
                return Properties.Settings.Default.BuildDestPath + "\\" + dir.Name;
            }
            syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
            return null;
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
            if (Properties.Settings.Default.RunOptima)
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

        public bool SOARequirementsAreMet()
        {
            if (!validatorUI.DestSOAPathIsValid())
            {
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return false;
            }

            if (!windowsService.DoesSOAServiceExist())
            {
                Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!windowsService.SoaIsStopped())
            {
                Log.Error(Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
