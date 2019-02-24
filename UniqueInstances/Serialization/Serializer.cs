using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using ColossalFramework.IO;

namespace UniqueInstances
{
    public class Serializer
    {
        private static string ConfigurationPath => Path.Combine(DataLocation.localApplicationData, "UniqueInstances.udt");

        public static Data UniqueData { get => Manager.Instance.Data; private set =>  Manager.Instance.Data = value; }

        internal static bool Save()
        {
            string path = ConfigurationPath;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    binaryFormatter.Serialize(memoryStream, UniqueData);
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
                }
                return true;
            }
        }
        internal static void Load()
        {
            string path = ConfigurationPath;
            byte[] array = File.ReadAllBytes(path);
            Data uniqueData = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                try
                {
                    memoryStream.Write(array, 0, array.Length);
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    uniqueData = (Data)binaryFormatter.Deserialize(memoryStream);
                }
                catch (Exception exception)
                {
                    Debug.LogWarning(exception);
                }
                finally
                {
                    if (uniqueData == null) UniqueData = new Data();
                    else UniqueData = uniqueData;
                }
            }
        }
    }
}
