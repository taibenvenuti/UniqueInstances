using ColossalFramework.Math;
using System;

namespace UniqueInstances
{
    [Serializable]
    public class UniqueTree
    {
        public string UniqueName { get; set; }
        public string UniqueDescription { get; set; }
        public string OriginalName { get; private set; }
        public UniqueTreeInfo UniqueInfo { get; private set; }
        public Randomizer Randomizer { get; set; }

        public UniqueTree(TreeInfo newInfo, string oldName, string newName, string newDescription)
        {
            UniqueName = newName;
            UniqueDescription = newDescription;
            OriginalName = oldName;
            UniqueInfo = new UniqueTreeInfo(newInfo);
        }

        /// <summary>
        /// Must be called after Data has been initialized.
        /// Sets saved custom 'properties' to a unique TreeInfo.
        /// </summary>
        public void Load()
        {
            UniqueInfo.Load(PrefabCollection<TreeInfo>.FindLoaded(UniqueName));
        }

        public void CreateCopy()
        {
            PrefabCollection<TreeInfo>.FindLoaded(OriginalName).CopyInfo(UniqueName);
        }

        public void CopyFields()
        {
            PrefabCollection<TreeInfo>.FindLoaded(OriginalName).CopyFields(UniqueName);
        }
    }
}
