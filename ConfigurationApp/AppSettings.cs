using OptimaSync.Confiugration;

namespace OptimaSync.ConfigurationApp
{
    public class AppSettings
    {
        PathSetup pathSetup = new PathSetup();
        public void SetPaths(string SourcePath, string DestPath, string OptimaSOAPath)
        {
            Properties.Settings.Default.BuildSourcePath = SourcePath;
            Properties.Settings.Default.BuildDestPath = DestPath;
            Properties.Settings.Default.BuildSOAPath = OptimaSOAPath;

            Properties.Settings.Default.Save();
        }

        public string GetSourcePath()
        {
            return pathSetup.BuildSourcePathProp;
        }

        public string GetDestPath()
        {
            return pathSetup.BuildDestPathProp;
        }

        public string GetOptimaSOAPath()
        {
            return pathSetup.BuildSOAPathProp;
        }
    }
}
