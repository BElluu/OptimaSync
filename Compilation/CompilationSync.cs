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
        PathSetup pathSetup = new PathSetup();
        Settings settings = new Settings();

/*        const string compilationSourcePath = @"H:\OptimaSync\KatalogKompilacji\";
        const string compilationDestPath = @"H:\OptimaSync\KomputerPracownika\";*/

        public void DownloadLatestCompilation()
        {
            var dir = FindLastCompilation();

            dir.MoveTo(settings.GetDestPath() + "\\" + dir.Name);
        }

        private DirectoryInfo FindLastCompilation()
        {

            if (string.IsNullOrEmpty(settings.GetSourcePath()))
            {
                throw new NullReferenceException("Ustaw lokalizację z której chcesz pobierać pliki kompilacji");
            }
            var directory = new DirectoryInfo(settings.GetSourcePath());
            var lastCompilation = directory.GetDirectories()
                .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                            !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(f => f.LastWriteTime)
                .First();

            return lastCompilation;
        }
    }
}
