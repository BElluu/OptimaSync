namespace OptimaSync.Confiugration
{
    public class PathSetup
    {
        private string buildSourcePath;
        private string buildDestPath;
        private string buildSOAPath;

        public string BuildSourcePathProp
        {
            get { return buildSourcePath; }
            set { buildSourcePath = value; }
        }

        public string BuildDestPathProp
        {
            get { return buildDestPath; }
            set { buildDestPath = value; }
        }

        public string BuildSOAPathProp
        {
            get { return buildSOAPath; }
            set { buildSOAPath = value; }
        }
    }
}
