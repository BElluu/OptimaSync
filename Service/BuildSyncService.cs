using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OptimaSync.Constants;
using Serilog;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        WindowsService windowsService = new WindowsService();

        public void PrepareOptimaBuild(bool withSoa)
        {
            if (withSoa)
            {
                RegisterOptima(DownloadLatestBuildWithSOA());
            }
            else
            {
                RegisterOptima(DownloadLatestBuild());
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

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildDestPath))
            {
                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }

            if (Directory.Exists(dirDest))
            {
                MessageBox.Show(Messages.YOU_HAVE_LATEST_BUILD, Messages.YOU_HAVE_LATEST_BUILD_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            try
            {
                FileSystem.CopyDirectory(dir.ToString(), dirDest);
                Log.Information("Skopiowano " + dir.Name);
                return dirDest;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        public string DownloadLatestBuildWithSOA()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildSOAPath))
            {
                Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
            }

            if (!windowsService.DoesSOAServiceExist())
            {
                Log.Error(Messages.SOA_SERVICE_DONT_EXIST);
                MessageBox.Show(Messages.SOA_SERVICE_DONT_EXIST, Messages.SOA_SERVICE_DONT_EXIST_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (windowsService.StopSOAService() != 0)
            {
                Log.Error(Messages.SOA_SERVICE_NOT_STOPPED);
                MessageBox.Show(Messages.SOA_SERVICE_NOT_STOPPED, Messages.SOA_SERVICE_NOT_STOPPED_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var dir = FindLastBuild();

            if (dir == null)
            {
                return null;
            }

            var dirDestSOA = Properties.Settings.Default.BuildSOAPath;

            /*            if (Directory.Exists(dirDestSOA))
                        {
                            MessageBox.Show(Messages.YOU_HAVE_LATEST_BUILD, Messages.YOU_HAVE_LATEST_BUILD_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return null;
                        }*/

            try
            {

                foreach (string dirPath in Directory.GetDirectories(dir.ToString(), "*", System.IO.SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(dir.ToString(), dirDestSOA));
                }

                foreach (string newPath in Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(dir.ToString(), dirDestSOA), true);
                }

                Log.Information("Skopiowano " + dir.Name);
                return dirDestSOA;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }

            /*            var dir = FindLastBuild();

                        if (dir == null)
                        {
                            return null;
                        }

                        var dirDestSOA = Properties.Settings.Default.BuildSOAPath + "\\" + dir.Name;

                        if (string.IsNullOrEmpty(Properties.Settings.Default.BuildSOAPath))
                        {
                            Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                            throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                        }

                        try
                        {
                            FileSystem.CopyDirectory(dir.ToString(), dirDestSOA);
                            Log.Information("Skopiowano " + dir.Name);
                            return dirDestSOA;

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            return null;
                        }*/
        }

        private DirectoryInfo FindLastBuild()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildSourcePath))
            {
                Log.Error(Messages.BUILD_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.BUILD_PATH_CANNOT_BE_EMPTY); 
            }

            try
            {
                var directory = new DirectoryInfo(Properties.Settings.Default.BuildSourcePath);
                var lastCompilation = directory.GetDirectories()
                    .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("rar", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("FIXES", StringComparison.InvariantCultureIgnoreCase)) // TODO Excluded directories to list
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastCompilation;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(Messages.BUILD_PATH_DONT_HAVE_ANY_BUILD, Messages.BUILD_PATH_DONT_HAVE_ANY_BUILD_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void RegisterOptima(string path)
        {
            if(path == null)
            {
                return;
            }

            Process proc;
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.FileName = "Rejestr.bat";
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();
                Log.Information(Messages.OPTIMA_REGISTERED);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(Messages.REGISTER_OPTIMA_ERROR, Messages.REGISTER_OPTIMA_ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
