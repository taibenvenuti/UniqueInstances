using ICities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UniqueInstances
{
    public class SerializableDataExtension : SerializableDataExtensionBase
    {
        private const string DATA_ID = "Unique_Instances_DATA";
        public InstancesData Data { get => Manager.Instance.InstancesData; set => Manager.Instance.InstancesData = value; }

        public override void OnSaveData()
        {
            base.OnSaveData();

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, Data);
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
                Data = binaryFormatter.Deserialize(memoryStream) as InstancesData;
            }
        }
    }
}
