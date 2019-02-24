using ColossalFramework;
using ColossalFramework.Math;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UniqueInstances.Extensions;
using UnityEngine;

namespace UniqueInstances.Data.Trees
{
    [Serializable]
    public struct UniqueTree
    {
        public ulong UniqueID { get; private set; }
        public string UniqueName { get; set; }
        public string UniqueDescription { get; set; }
        public string OriginalName { get; private set; }
        public UniqueTreeInfo UniqueInfo { get; private set; }
        public Randomizer Randomizer { get; set; }

        public int PrefabDataIndex
        {
            get
            {
                if (string.IsNullOrEmpty(UniqueName))
                {
                    TreeInfo original = PrefabCollection<TreeInfo>.FindLoaded(OriginalName);
                    if (original == null) return -1;
                    return original.m_prefabDataIndex;
                }
                TreeInfo copy = PrefabCollection<TreeInfo>.FindLoaded(UniqueName);
                if (copy == null) return -1;
                return copy.m_prefabDataIndex;
            }
        }

        public UniqueTree(uint treeID, string newName, string newDescription, bool replace)
        {
            UniqueID = (ulong)DateTime.Now.Ticks + treeID;
            UniqueName = newName;
            UniqueDescription = newDescription;

            TreeInfo oldInfo = TreeManager.instance.m_trees.m_buffer[treeID].Info;
            TreeInfo newInfo = TreeManager.instance.m_trees.m_buffer[treeID].GetCopyOf(newName);

            OriginalName = oldInfo.name;
            UniqueInfo = new UniqueTreeInfo(newInfo);
            Randomizer = new Randomizer(UniqueID);

            if (replace)
            {
                TreeManager.instance.m_trees.m_buffer[treeID].Info = newInfo;
                TreeManager.instance.m_trees.m_buffer[treeID].UpdateTree(treeID);
                TreeManager.instance.UpdateTreeRenderer(treeID, true);
                TreeManager.instance.UpdateTree(treeID);
            }
        }
    }
}
