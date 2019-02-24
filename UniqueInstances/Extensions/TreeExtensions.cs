using System;
using System.Reflection;
using UniqueInstances.Data.Buildings;
using static UnityEngine.Object;

namespace UniqueInstances.Extensions
{
    public static partial class Extensions
    {
        public static TreeInfo GetCopyOf(this TreeInstance treeInstance, string newName)
        {
            int prefabCount = PrefabCollection<TreeInfo>.LoadedCount();
            if (++prefabCount <= UniqueBuildingsData.MAX_UNIQUE_COUNT)
            {
                TreeInfo oldPrefab = treeInstance.Info;
                TreeInfo newPrefab = Instantiate(oldPrefab);
                
                newPrefab.gameObject.name = newName;
                newPrefab.gameObject.SetActive(false);

                newPrefab.m_variations = new TreeInfo.Variation[oldPrefab.m_variations.Length];
                oldPrefab.m_variations.CopyTo(newPrefab.m_variations, 0);

                PrefabCollection<TreeInfo>.InitializePrefabs("Custom Assets", newPrefab, null);
                PrefabCollection<TreeInfo>.BindPrefabs();
                TreeInfo treeInfo = PrefabCollection<TreeInfo>.FindLoaded(newName);

                foreach (FieldInfo fieldInfo in treeInfo.GetType().GetFields(Flags))
                {
                    string fieldName = fieldInfo.Name;
                    if (fieldName == "m_prefabDataIndex") continue;
                    object newValue = oldPrefab.GetType().GetField(fieldName, Flags).GetValue(oldPrefab);
                    treeInfo.GetType().GetField(fieldName, Flags).SetValue(treeInfo, newValue);
                }
                return treeInfo;
            }
            throw new Exception("Maximum number of prefab instances reached!");
        }
    }
}
