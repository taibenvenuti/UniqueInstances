using System;
using System.Reflection;
using static UnityEngine.Object;

namespace UniqueInstances
{
    public static partial class Extensions
    {
        public static UniqueTree GetImmediateCopy(this TreeInfo tree, string newName, string newDescription = "", uint treeID = 0)
        {
            string oldName = tree.name;
            tree.CopyInfo(newName);
            PrefabCollection<BuildingInfo>.BindPrefabs();
            tree.CopyFields(newName);
            TreeInfo newInfo = PrefabCollection<TreeInfo>.FindLoaded(newName);
            if (treeID != 0)
            {
                TreeManager.instance.m_trees.m_buffer[treeID].Info = newInfo;
                TreeManager.instance.m_trees.m_buffer[treeID].UpdateTree(treeID);
                TreeManager.instance.UpdateTreeRenderer(treeID, true);
                TreeManager.instance.UpdateTree(treeID);
            }
            return new UniqueTree(newInfo, oldName, newName, newDescription);
        }

        public static void CopyInfo(this TreeInfo oldInfo, string newName)
        {
            int prefabCount = PrefabCollection<TreeInfo>.LoadedCount();
            if (++prefabCount <= Data.MAX_UNIQUE_COUNT)
            {
                TreeInfo newInfo = Instantiate(oldInfo);

                newInfo.gameObject.name = newName;
                newInfo.gameObject.SetActive(false);

                PrefabCollection<TreeInfo>.InitializePrefabs("Custom Assets", newInfo, null);
            }
            else throw new Exception("Maximum number of prefab instances reached!");
        }

        public static void CopyFields(this TreeInfo oldInfo, string newName)
        {
            TreeInfo newInfo = PrefabCollection<TreeInfo>.FindLoaded(newName);

            if (newInfo == null) return;

            newInfo.m_variations = new TreeInfo.Variation[oldInfo.m_variations.Length];
            oldInfo.m_variations.CopyTo(newInfo.m_variations, 0);

            foreach (FieldInfo fieldInfo in newInfo.GetType().GetFields(Flags))
            {
                string fieldName = fieldInfo.Name;
                if (fieldName == "m_prefabDataIndex") continue;
                object newValue = oldInfo.GetType().GetField(fieldName, Flags).GetValue(oldInfo);
                newInfo.GetType().GetField(fieldName, Flags).SetValue(newInfo, newValue);
            }
        }
    }
}
