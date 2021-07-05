using System;
using System.IO;
using System.Linq;
using OptimaSync.ConfigurationApp;
using OptimaSync.Constants;
using Serilog;

namespace OptimaSync.Service
{
    public class BuildSync
    {
        AppSettings appSettings = new AppSettings();

        public void DownloadLatestCompilation()
        {
            var dir = FindLastCompilation();

            try
            {
                dir.MoveTo(Properties.Settings.Default.BuildDestPath + "\\" + dir.Name);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private DirectoryInfo FindLastCompilation()
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
