using ICities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UniqueInstances
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        private const string DATA_ID = "Unique_Instances_DATA";
        public Data.SerializableInstances InstancesData { get => Manager.Instance.Data.GetSerializableInstances(); set => Manager.Instance.Data.LoadDeserializedInstances(value); }

        public override void OnSaveData()
        {
            base.OnSaveData();

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, InstancesData);
                serializableDataManager.SaveData(DATA_ID, memoryStream.ToArray());
            }
        }

        public override void OnLoadData()
        {
            base.OnLoadData();
            var data = serializableDataManager.LoadData(DATA_ID);
            if (data == null || data.Length == 0) return;
            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream(data))
            {
                InstancesData = binaryFormatter.Deserialize(memoryStream) as Data.SerializableInstances;
            }
        }
    }
}
