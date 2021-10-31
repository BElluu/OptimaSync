using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OptimaSync.Helper
{
    public class SearchBuildServiceHelper
    {
        public string GetProgrammerVersion()
        {
            string programmerCommonDllPath = AppConfigHelper.GetConfigValue("ProgrammerDestination") + "\\" + BuildSyncServiceHelper.CHECK_VERSION_FILE;

            if (!File.Exists(programmerCommonDllPath))
            {
                return "None";
            }

            FileVersionInfo programmerCommonDllFile = FileVersionInfo.GetVersionInfo(programmerCommonDllPath);
            string programmerCommonDllVerson = programmerCommonDllFile.ProductVersion.ToString();

            return programmerCommonDllVerson;
        }

        public string GetSoaVersion()
        {
            string soaCommonDllPath = AppConfigHelper.GetConfigValue("SOADestination") + "\\" + BuildSyncServiceHelper.CHECK_VERSION_FILE;

            if (!File.Exists(soaCommonDllPath))
            {
                return "None";
            }

            FileVersionInfo soaCommonDllFile = FileVersionInfo.GetVersionInfo(soaCommonDllPath);
            string soaCommonDllVerson = soaCommonDllFile.ProductVersion.ToString();

            return soaCommonDllVerson;
        }

        public string GetBasicVersion()
        {

            string lastBuildCommonDllPath = null;
            var directory = new DirectoryInfo(AppConfigHelper.GetConfigValue("Destination"));
            var lastBuild = directory.GetDirectories()
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            if (lastBuild != null) 
            { 
            lastBuildCommonDllPath = lastBuild.ToString() + "\\" + BuildSyncServiceHelper.CHECK_VERSION_FILE;
            }

            if (!File.Exists(lastBuildCommonDllPath))
            {
                return "None";
            }
            FileVersionInfo lastBuildCommonDllFile = FileVersionInfo.GetVersionInfo(lastBuildCommonDllPath);
            string lastBuildCommonDllVersion = lastBuildCommonDllFile.ProductVersion.ToString();

            return lastBuildCommonDllVersion;
        }
    }
}
