using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OptimaSync.Config
{
    public static class ConfigAppCreator
    {
        public static void Create()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OptimaSync.dll.config"))
            {
                return;
            }

            XmlSerializer ser = new XmlSerializer(typeof(ConfigurationAppModel));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            TextWriter writer = new StreamWriter("OptimaSync.dll.config");
            ConfigurationAppModel configurationApp = new ConfigurationAppModel();

            AppSettings appSettings = new AppSettings()
            {
                Add = new Add[] {
                    new Add(){key = "DownloadType", value = "BASIC"},
                    new Add(){key = "RunOptima", value = "false"},
                    new Add(){key = "LatestVersionChecked", value = ""},
                    new Add(){key = "ProductionPath", value = "\\\\columbia\\tmp\\Bartlomiej.Komendarczuk\\OSA\\Optima_Wersje_Prod"},
                    new Add(){key = "CompilationPath", value = "\\\\natalie\\cdnkop-gotowe\\archiwum.optima\\"},
                    new Add(){key = "eDeclarationPath", value = "\\natalie\\cdnKOP-gotowe\\Archiwum\\Deklaracje"},
                    new Add(){key = "Destination", value = ""},
                    new Add(){key = "SOADestination", value = ""},
                    new Add(){key = "ProgrammerDestination", value = "D:\\Optima"},
                    new Add(){key = "AutoCheckVersion", value = "false"},
                    new Add(){key = "NotificationSound", value = "false"},
                    new Add(){key = "DownloadEDeclaration", value = "false"},
                    new Add(){key = "BuildServer", value = "Natalie"},
                    new Add(){key = "ProductionServer", value = "Columbia"},
                    new Add(){key = "eDeclarationServer", value = "Columbia"}
                }.ToList()
            };
            configurationApp.appSettings = appSettings;
            ser.Serialize(writer, configurationApp, ns);
            writer.Close();
        }
    }
}
