using System;
using System.IO;
using System.Linq;
using OptimaSync.Constant;
using Serilog;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.Common;
using System.Diagnostics;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        static string[] excludedStrings = { "CIV", "SQL", "test", "rar", "FIXES" };

        SyncUI syncUI = new SyncUI();
        RegisterDLLService registerDLL = new RegisterDLLService();
        ValidatorUI validatorUI = new ValidatorUI();

        public void PrepareOptimaBuild(bool withSoa, bool isProgrammer)
        {
            syncUI.EnableElementsOnForm(false);
            registerDLL.RegisterOptima(DownloadBuild(isProgrammer, withSoa), isProgrammer);
            syncUI.EnableElementsOnForm(true);
        }

        public string DownloadBuild(bool isProgrammer, bool withSOA)
        {
            string extractionPath;

            if (isProgrammer)
            {
                extractionPath = Properties.Settings.Default.ProgrammersPath;
            }
            else if (withSOA)
            {
                if (validatorUI.SOARequirementsAreMet() == false)
                {
                    return null;
                }
                extractionPath = Properties.Settings.Default.BuildSOAPath;
            }
            else
            {
                extractionPath = Properties.Settings.Default.BuildDestPath;
            }

            var dir = FindLastBuild();
            if (dir == null)
            {
                return null;
            }

            if (BuildVersionsAreSame(dir.ToString(), isProgrammer, withSOA, dir.Name))
            {
                MessageBox.Show(Messages.YOU_HAVE_LATEST_BUILD, Messages.INFORMATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Information(Messages.YOU_HAVE_LATEST_BUILD);
                syncUI.ChangeProgressLabel(Messages.OSA_READY_TO_WORK);
                return null;
            }

            try
            {
                if (isProgrammer || withSOA)
                {
                    syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                    foreach (string dirPath in Directory.GetDirectories(dir.ToString(), "*", System.IO.SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(dir.ToString(), extractionPath));
                    }

                    foreach (string newPath in Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(dir.ToString(), extractionPath), true);
                    }

                    Log.Information("Skopiowano " + dir.Name);
                    return extractionPath;
                }
                else
                {
                    string newBuildDirectory = extractionPath + "\\" + dir.Name;
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(newBuildDirectory);

                    syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                    foreach (string dirPath in Directory.GetDirectories(dir.ToString(), "*", System.IO.SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(dir.ToString(), newBuildDirectory));
                    }

                    foreach (string newPath in Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        File.Copy(newPath, newPath.Replace(dir.ToString(), newBuildDirectory), true);
                    }
                    Log.Information("Skopiowano " + dir.Name);
                    return newBuildDirectory;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                return null;
            }

        }

        private DirectoryInfo FindLastBuild()
        {

            if (!NetworkDrive.HaveAccessToHost("natalie"))
            {
                MessageBox.Show(Messages.ACCESS_TO_HOST_ERROR, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(Messages.ACCESS_TO_HOST_ERROR);
                return null;
            }
            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(Properties.Settings.Default.BuildSourcePath);
                var lastBuild = directory.GetDirectories()
                    .Where(q => excludedStrings.All(c => !q.Name.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
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

        private bool BuildVersionsAreSame(string buildPath, bool isProgrammer, bool withSOA, string buildDirectoryName)
        {
            string destCommonDllPath;
            if (isProgrammer)
            {
                destCommonDllPath = Properties.Settings.Default.ProgrammersPath + "\\" + "Common.dll";
            }
            else if (withSOA)
            {
                destCommonDllPath = Properties.Settings.Default.BuildSOAPath + "\\" + "Common.dll";
            }
            else
            {
                destCommonDllPath = Properties.Settings.Default.BuildDestPath + "\\" + buildDirectoryName + "\\" + "Common.dll";
            }

            if (!File.Exists(destCommonDllPath))
            {
                return false;
            }

            string sourceCommonDllPath = buildPath + "\\" + "Common.dll";
            FileVersionInfo sourceCommonDll = FileVersionInfo.GetVersionInfo(sourceCommonDllPath);
            string sourceCommonDllVersion = sourceCommonDll.ProductVersion.ToString();
            Console.WriteLine(sourceCommonDllVersion);

            FileVersionInfo destCommonDll = FileVersionInfo.GetVersionInfo(destCommonDllPath);
            string destCommonDllVersion = destCommonDll.ProductVersion.ToString();
            Console.WriteLine(destCommonDllVersion);

            if (sourceCommonDllVersion == destCommonDllVersion)
            {
                return true;
            }
            return false;
        }
    }
}
