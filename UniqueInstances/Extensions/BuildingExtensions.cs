using System;
using System.Reflection;
using static UnityEngine.Object;

namespace UniqueInstances
{
    public static partial class Extensions
    {
        public static BuildingInfo GetImmediateCopy(this BuildingInfo building, string newName)
        {
            building.CopyInfo(newName);
            PrefabCollection<BuildingInfo>.BindPrefabs();
            building.CopyFields(newName);
            return PrefabCollection<BuildingInfo>.FindLoaded(newName);
        }

        public static void CopyInfo(this BuildingInfo oldInfo, string newName)
        {
            int prefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
            if (++prefabCount <= Data.MAX_UNIQUE_COUNT)
            {
                BuildingInfo newInfo = Instantiate(oldInfo);

                newInfo.gameObject.name = newName;
                newInfo.gameObject.SetActive(false);

                PrefabCollection<BuildingInfo>.InitializePrefabs("Custom Assets", newInfo, null);
            }
            throw new Exception("Maximum number of prefab instances reached!");
        }

        public static void CopyFields(this BuildingInfo oldInfo, string newName)
        {
            BuildingInfo newInfo = PrefabCollection<BuildingInfo>.FindLoaded(newName);

            if (newInfo == null) return;

            BuildingAI oldAI = newInfo.GetComponent<BuildingAI>();
            BuildingAI newAI = newInfo.gameObject.AddComponent<BuildingAI>().GetCopyOf(oldAI);
            Destroy(oldAI);

            newInfo.m_props = new BuildingInfo.Prop[oldInfo.m_props.Length];
            oldInfo.m_props.CopyTo(newInfo.m_props, 0);

            foreach (FieldInfo fieldInfo in newInfo.GetType().GetFields(Flags))
            {
                string fieldName = fieldInfo.Name;
                if (fieldName == "m_prefabDataIndex" || fieldName == "WidthCount" || fieldName == "LengthCount") continue;
                object newValue = oldInfo.GetType().GetField(fieldName, Flags).GetValue(oldInfo);
                if (fieldName == "m_buildingAI")
                    newValue = newAI;
                newInfo.GetType().GetField(fieldName, Flags).SetValue(newInfo, newValue);
            }
        }
    }
}
