using System;
using System.IO;
using System.Linq;
using OptimaSync.Constants;
using Serilog;

namespace OptimaSync.Service
{
    public class BuildSync
    {
        public void DownloadLatestBuild()
        {
            var dir = FindLastBuild();

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildDestPath))
            {
                Log.Error(Messages.DEST_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.DEST_PATH_CANNOT_BE_EMPTY);
            }

            try
            {
                dir.MoveTo(Properties.Settings.Default.BuildDestPath + "\\" + dir.Name);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public void DownloadLatestBuildWithSOA()
        {
            var dir = FindLastBuild();

            if (string.IsNullOrEmpty(Properties.Settings.Default.BuildSOAPath))
            {
                Log.Error(Messages.SOA_PATH_CANNOT_BE_EMPTY);
                throw new NullReferenceException(Messages.SOA_PATH_CANNOT_BE_EMPTY);
            }

            try
            {
                dir.MoveTo(Properties.Settings.Default.BuildSOAPath + "\\" + dir.Name);
            }catch (Exception ex)
            {
                Log.Error(ex.Message);
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
    }
}
