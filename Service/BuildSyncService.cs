using System;
using System.IO;
using System.Linq;
using OptimaSync.Constant;
using Serilog;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using OptimaSync.UI;
using OptimaSync.Common;

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
            if (isProgrammer)
            {
                SyncUI.Invoke(() => MainForm.Instance.downloadBuildButton.Enabled = false);
                registerDLL.RegisterOptima(DownloadLatestBuildExtractFiles(isProgrammer),isProgrammer);
                SyncUI.Invoke(() => MainForm.Instance.downloadBuildButton.Enabled = true);
            }
            else
            {
                syncUI.EnableElementsOnForm(false);
                if (withSoa)
                {
                    registerDLL.RegisterOptima(DownloadLatestBuildExtractFiles(false), false);
                }
                else
                {
                    registerDLL.RegisterOptima(DownloadLatestBuild(), false);
                }
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
                syncUI.ChangeProgressLabel(Messages.PENDING);
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }

            if (Directory.Exists(dirDest))
            {
                MessageBox.Show(Messages.YOU_HAVE_LATEST_BUILD, Messages.INFORMATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                syncUI.ChangeProgressLabel(Messages.PENDING);
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

            if (isProgrammer)
            {
                extractionPath = Properties.Settings.Default.ProgrammersPath;
            }
            else
            {
                if (validatorUI.SOARequirementsAreMet() == false)
                {
                    return null;
                }

                extractionPath = Properties.Settings.Default.BuildSOAPath;
            }

            var dir = FindLastBuild();

            if (dir == null)
            {
                return null;
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
    }
}
