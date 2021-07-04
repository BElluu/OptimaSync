using System;
using System.IO;
using System.Linq;
using OptimaSync.ConfigurationApp;

namespace OptimaSync.Service
{
    public class CompilationSync
    {
        AppSettings appSettings = new AppSettings();

        public void DownloadLatestCompilation()
        {
            var dir = FindLastCompilation();

            try
            {
                dir.MoveTo(appSettings.GetDestPath() + "\\" + dir.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace); // TODO logger
            }
        }

        private DirectoryInfo FindLastCompilation()
        {

            if (string.IsNullOrEmpty(appSettings.GetSourcePath()))
            {
                throw new NullReferenceException("Ustaw lokalizację z której chcesz pobierać pliki kompilacji");
            }
            try
            {
                var directory = new DirectoryInfo(appSettings.GetSourcePath());
                var lastCompilation = directory.GetDirectories()
                    .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                                !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();

                return lastCompilation;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace); // TODO logger
                return null;
            }
        }
    }
}
