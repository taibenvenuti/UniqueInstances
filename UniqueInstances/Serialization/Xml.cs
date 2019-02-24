using ColossalFramework.IO;
using System;
using System.IO;
using System.Xml.Serialization;
using UniqueInstances.Data;

namespace UniqueInstances.Serialization
{
    [XmlRoot("UniqueInstancesData")]
    public class Xml
    {
        [XmlIgnore]
        private static readonly string configurationPath = Path.Combine(DataLocation.localApplicationData, "UniqueInstancesData.xml");

        public UniqueSaveGameData UniqueXmlDataModel { get; set; }

        public static string ConfigurationPath
        {
            get
            {
                return configurationPath;
            }
        }

        public Xml() { }

        public void OnPreSerialize() { }

        public void OnPostDeserialize() { }

        public void Save()
        {
            var fileName = ConfigurationPath;
            var config = UniqueMod.Settings;
            var serializer = new XmlSerializer(typeof(Xml));
            using (var writer = new StreamWriter(fileName))
            {
                config.OnPreSerialize();
                serializer.Serialize(writer, config);
            }
        }


        public static Xml Load()
        {
            var fileName = ConfigurationPath;
            var serializer = new XmlSerializer(typeof(Xml));
            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    var config = serializer.Deserialize(reader) as Xml;
                    return config;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"Error Parsing {fileName}: {ex}");
                return null;
            }
        }
    }
}
