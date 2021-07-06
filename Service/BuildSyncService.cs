using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OptimaSync.Constants;
using Serilog;
using Microsoft.VisualBasic.FileIO;

namespace OptimaSync.Service
{
    public class BuildSyncService
    {
        WindowsService windowsService = new WindowsService();

        public void PrepareOptimaBuild(bool withSoa)
        {
            if (withSoa)
            {
                RegisterOptima(DownloadLatestBuildWithSOA(), true);
            }
            else
            {
                RegisterOptima(DownloadLatestBuild(), false);
            }
        }

/*        public string DLB()
        {
            var dir = FindLastBuild();
            var dirDest = Properties.Settings.Default.BuildDestPath + "\\" + dir.Name;

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildDestPath))
            {
                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }
            try
            {
                Directory.Move(dir.ToString(), dirDest);
                Log.Information("Skopiowano " + dir.Name);
                return dirDest;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }*/
        public string DownloadLatestBuild()
        {
            var dir = FindLastBuild();
            var dirDest = Properties.Settings.Default.BuildDestPath + "\\" + dir.Name;

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildDestPath))
            {
                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }

            try
            {
                /*dir.MoveTo(dirDest);*/
                /*Directory.Move(dir.ToString(), dirDest);*/
/*                FileSystem.MoveDirectory(dir.ToString(), dirDest);*/
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
            var dir = FindLastBuild();
            var dirDestSOA = Properties.Settings.Default.BuildSOAPath + "\\" + dir.Name;

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildSOAPath))
            {
                Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
            }

            try
            {
                dir.MoveTo(dirDestSOA);
                Log.Information("Skopiowano " + dir.Name);
                return dirDestSOA;
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
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
                                !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastCompilation;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        private void RegisterOptima(string path, bool withSoa)
        {
            Process proc;

            if (withSoa) {
                windowsService.StopSOAService();
            }
            try
            {
                proc = new Process();
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.FileName = "Rejestr.bat"; // TODO File not exist but is in directory...
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit();
                Log.Information(Messages.OPTIMA_REGISTERED);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
