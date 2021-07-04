using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimaSync.Confiugration;
using OptimaSync.ConfigurationApp;

namespace OptimaSync.Compilation
{
    public class CompilationSync
    {
        AppSettings appSettings = new AppSettings();

        public void DownloadLatestCompilation()
        {
            var dir = FindLastCompilation();

            dir.MoveTo(appSettings.GetDestPath() + "\\" + dir.Name);
        }

        private DirectoryInfo FindLastCompilation()
        {

            if (string.IsNullOrEmpty(appSettings.GetSourcePath()))
            {
                throw new NullReferenceException("Ustaw lokalizację z której chcesz pobierać pliki kompilacji");
            }
            var directory = new DirectoryInfo(appSettings.GetSourcePath());
            var lastCompilation = directory.GetDirectories()
                .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                            !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(f => f.LastWriteTime)
                .First();

            return lastCompilation;
        }
    }
}
