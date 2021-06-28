using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimaSync.Confiugration;

namespace OptimaSync.Compilation
{
    public class CompilationSync
    {
        PathSetup pathSetup = new PathSetup();
        public void DownloadLastCompilation(String compilationSourcePath)
        {
          compilationSourcePath = pathSetup.GetCompilationSourcePath;

            if (string.IsNullOrEmpty(compilationSourcePath){
                throw new NullReferenceException("Ustaw lokalizację z której chcesz pobierać pliki kompilacji");
            }

        }
    }
}
