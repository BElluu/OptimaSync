using System.Collections.Generic;
using System.Xml.Serialization;

namespace OptimaSync.Config
{
    public struct Add
    {
        [XmlAttribute("key")]
        public string key { get; set; }

        [XmlAttribute("value")]
        public string value { get; set; }
    }


    [XmlRoot("configuration")]
    public class ConfigurationAppModel
    {
        [XmlElement("appSettings")]
        public AppSettings appSettings { get; set; }
    }

    public class AppSettings
    {
        [XmlElement("add")]
        public List<Add> Add { get; set; }
    }
}
