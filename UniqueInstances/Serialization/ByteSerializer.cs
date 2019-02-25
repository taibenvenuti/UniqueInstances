using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using ColossalFramework.IO;

namespace UniqueInstances
{
    public class ByteSerializer
    {
        private const string FileName = "Unique.idt";
        private static string FilePath => Path.Combine(DataLocation.localApplicationData, FileName);

        public static Data.SerializableInfos InfosData { get => Manager.Instance.Data.GetSerializableInfos(); private set =>  Manager.Instance.Data.LoadDeserializedInfos(value); }

        internal static bool Save()
        {
            string path = FilePath;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    binaryFormatter.Serialize(memoryStream, InfosData);
                    byte[] bytes = memoryStream.ToArray();
                    File.WriteAllBytes(path, bytes);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                    return false;
                }
                finally
                {
                    Debug.Log($"Successfully saved {InfosData.GetType().Name} to {FilePath}");
                }
                return true;
            }
        }
        internal static void Load()
        {
            string path = FilePath;
            Data.SerializableInfos infosData = null;
            if(File.Exists(FilePath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    try
                    {
                        byte[] array = File.ReadAllBytes(path);
                        memoryStream.Write(array, 0, array.Length);
                        memoryStream.Seek(0L, SeekOrigin.Begin);
                        infosData = (Data.SerializableInfos)binaryFormatter.Deserialize(memoryStream);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogWarning($"Exception caught: {exception}");
                    }
                    finally
                    {
                        if (infosData == null) InfosData = new Data.SerializableInfos(Manager.Instance.Data);
                        else InfosData = infosData;
                    }
                }
            }
            else
            {
                Manager.Instance.Data = new Data();
            }
        }
    }
}
