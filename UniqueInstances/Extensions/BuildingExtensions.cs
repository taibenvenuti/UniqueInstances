using System;
using System.Reflection;
using static UnityEngine.Object;
using UniqueInstances.Data.Buildings;

namespace UniqueInstances.Extensions
{
    public static partial class Extensions
    {
        public static BuildingInfo GetCopyOf(this Building building, string newUniqueName)
        {
            int prefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
            if (++prefabCount <= UniqueBuildingsData.MAX_UNIQUE_COUNT)
            {
                BuildingInfo oldPrefab = building.Info;
                BuildingInfo newPrefab = Instantiate(oldPrefab);

                newPrefab.gameObject.name = newUniqueName;
                newPrefab.gameObject.SetActive(false);

                BuildingAI oldAI = newPrefab.GetComponent<BuildingAI>();
                BuildingAI newAI = newPrefab.gameObject.AddComponent<BuildingAI>().GetCopyOf(oldAI);
                Destroy(oldAI);

                newPrefab.m_props = new BuildingInfo.Prop[oldPrefab.m_props.Length];
                oldPrefab.m_props.CopyTo(newPrefab.m_props, 0);

                PrefabCollection<BuildingInfo>.InitializePrefabs("Custom Assets", newPrefab, null);
                PrefabCollection<BuildingInfo>.BindPrefabs();
                BuildingInfo buildingInfo = PrefabCollection<BuildingInfo>.FindLoaded(newUniqueName);

                foreach (FieldInfo fieldInfo in buildingInfo.GetType().GetFields(Flags))
                {
                    string fieldName = fieldInfo.Name;
                    if (fieldName == "m_prefabDataIndex" || fieldName == "WidthCount" || fieldName == "LengthCount") continue;
                    object newValue = oldPrefab.GetType().GetField(fieldName, Flags).GetValue(oldPrefab);
                    if (fieldName == "m_buildingAI")
                        newValue = newAI;
                    buildingInfo.GetType().GetField(fieldName, Flags).SetValue(buildingInfo, newValue);
                }

                return buildingInfo;
            }
            throw new Exception("Maximum number of prefab instances reached!");
        }
    }
}
