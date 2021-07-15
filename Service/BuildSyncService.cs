using System;
using System.IO;
using System.Linq;
using OptimaSync.Constant;
using Serilog;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using OptimaSync.UI;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        WindowsService windowsService = new WindowsService();
        SyncUI syncUI = new SyncUI();
        RegisterDLLService registerDLL = new RegisterDLLService();
        ValidatorUI validatorUI = new ValidatorUI();

        public void PrepareOptimaBuild(bool withSoa, bool isProgrammer)
        {
            try
            {
                syncUI.EnableElementsOnForm(false);
                if (isProgrammer)
                {
                    DownloadLatestBuildExtractFiles(isProgrammer);
                }
                else
                {
                    if (withSoa)
                    {
                        registerDLL.RegisterOptima(DownloadLatestBuildExtractFiles(false), false);
                    }
                    else
                    {
                        registerDLL.RegisterOptima(DownloadLatestBuild(), false);
                    }
                }
            }
            finally
            {
                syncUI.EnableElementsOnForm(true);
            }
        }
        public string DownloadLatestBuild()
        {
            var dir = FindLastBuild();
            if (dir == null)
            {
                return null;
            }
            var dirDest = Properties.Settings.Default.BuildDestPath + "\\" + dir.Name;

            if (!validatorUI.DestPathIsValid())
            {
                syncUI.ChangeProgressLabel("Oczekuje...");
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }

            if (Directory.Exists(dirDest))
            {
                MessageBox.Show(Messages.YOU_HAVE_LATEST_BUILD, Messages.INFORMATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                syncUI.ChangeProgressLabel("Oczekuje...");
                return null;
            }

            try
            {
                syncUI.ChangeProgressLabel(Messages.DOWNLOADING_BUILD);
                FileSystem.CopyDirectory(dir.ToString(), dirDest);
                Log.Information("Skopiowano " + dir.Name);
                return dirDest;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                return null;
            }
        }

        public string DownloadLatestBuildExtractFiles(bool isProgrammer)
        {
            string extractionPath;

            var dir = FindLastBuild();

            if (dir == null)
            {
                return null;
            }

            if (isProgrammer)
            {
                extractionPath = Properties.Settings.Default.ProgrammersPath;
            }
            else
            {
                if (!validatorUI.DestSOAPathIsValid())
                {
                    syncUI.ChangeProgressLabel("Oczekuje...");
                    throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                }

                if (!windowsService.DoesSOAServiceExist())
                {
                    Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                    MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (windowsService.StopSOAService() != 0)
                {
                    Log.Error(Messages.SOA_SERVICE_NOT_STOPPED);
                    MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                extractionPath = Properties.Settings.Default.BuildSOAPath;
            }
            try
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
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                return null;
            }
        }

        private DirectoryInfo FindLastBuild()
        {
            try
            {
                syncUI.ChangeProgressLabel(Messages.SEARCHING_FOR_BUILD);
                var directory = new DirectoryInfo(Properties.Settings.Default.BuildSourcePath);
                var lastBuild = directory.GetDirectories()
                    .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("test", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("rar", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("FIXES", StringComparison.InvariantCultureIgnoreCase)) // TODO Excluded directories to list
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastBuild;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                syncUI.ChangeProgressLabel(Messages.ERROR_CHECK_LOGS);
                MessageBox.Show(Messages.BUILD_PATH_DONT_HAVE_ANY_BUILD, Messages.ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
