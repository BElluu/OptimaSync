using OptimaSync.Constant;
using OptimaSync.Service;
using OptimaSync.UI;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OptimaSync.Helper
{
    public class BuildSyncServiceHelper
    {
        public static readonly string LOCK_FILE = "osync.lock";
        public static readonly string CHECK_VERSION_FILE = "Common.dll";

        ValidatorUI validatorUI = new ValidatorUI();
        WindowsService windowsService = new WindowsService();
        SyncUI syncUI = new SyncUI();
        public bool BuildVersionsAreSame(string buildPath, bool isProgrammer, bool withSOA, string buildDirectoryName)
        {
            string destCommonDllPath;
            if (isProgrammer)
            {
                destCommonDllPath = Properties.Settings.Default.ProgrammersPath + "\\" + CHECK_VERSION_FILE;
            }
            else if (withSOA)
            {
                destCommonDllPath = Properties.Settings.Default.BuildSOAPath + "\\" + CHECK_VERSION_FILE;
            }
            else
            {
                destCommonDllPath = Properties.Settings.Default.BuildDestPath + "\\" + buildDirectoryName + "\\" + CHECK_VERSION_FILE;
            }

            if (!File.Exists(destCommonDllPath))
            {
                return false;
            }

            string sourceCommonDllPath = buildPath + "\\" + CHECK_VERSION_FILE;
            FileVersionInfo sourceCommonDll = FileVersionInfo.GetVersionInfo(sourceCommonDllPath);
            string sourceCommonDllVersion = sourceCommonDll.ProductVersion.ToString();

            FileVersionInfo destCommonDll = FileVersionInfo.GetVersionInfo(destCommonDllPath);
            string destCommonDllVersion = destCommonDll.ProductVersion.ToString();

            if (sourceCommonDllVersion == destCommonDllVersion)
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
            else if (withSOA)
            {
                if (SOARequirementsAreMet() == false)
                {
                    return null;
                }
                return Properties.Settings.Default.BuildSOAPath;
            }
            else
            {
                if (validatorUI.DestPathIsValid())
                {
                    return Properties.Settings.Default.BuildDestPath + "\\" + dir.Name;
                }
                return null;
            }
        }

        public bool DoesLockFileExist(string path)
        {
            if (File.Exists(path + "\\" + LOCK_FILE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RunOptima(string path)
        {
            if (Properties.Settings.Default.RunOptima == true)
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
                throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
            }

            if (!windowsService.DoesSOAServiceExist())
            {
                Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (windowsService.StopSOAService() != 0)
            {
                Log.Error(Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
