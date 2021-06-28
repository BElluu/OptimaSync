using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimaSync.Confiugration
{
    public class PathSetup
    {
        private string compilationSourcePath { get; set; }
        private string compilationDestPath { get; set; }
        private string compilationSOAPath { get; set; }

        public string GetCompilationSourcePath => compilationSourcePath;
        public string GetCompilationDestPath => compilationDestPath;
        public string GetCompilationSOAPath => compilationSOAPath;


    }
}
