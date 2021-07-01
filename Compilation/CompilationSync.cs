using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimaSync.Confiugration;

namespace OptimaSync.Compilation
{
    public class CompilationSync
    {
        PathSetup pathSetup = new PathSetup();

        const string compilationSourcePath = @"H:\OptimaSync\KatalogKompilacji\";
        const string compilationDestPath = @"H:\OptimaSync\KomputerPracownika\";

        public void DownloadLatestCompilation()
        {
            var dir = FindLastCompilation();

            dir.MoveTo(compilationDestPath + dir.Name);
        }

        private DirectoryInfo FindLastCompilation()
        {

            if (string.IsNullOrEmpty(compilationSourcePath))
            {
                throw new NullReferenceException("Ustaw lokalizację z której chcesz pobierać pliki kompilacji");
            }
            var directory = new DirectoryInfo(compilationSourcePath);
            var lastCompilation = directory.GetDirectories()
                .Where(q => !q.Name.Contains("CIV", StringComparison.InvariantCultureIgnoreCase) &&
                            !q.Name.Contains("SQL", StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(f => f.LastWriteTime)
                .First();

            return lastCompilation;
        }
    }
}
