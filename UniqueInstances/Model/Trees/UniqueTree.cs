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

        public UniqueTree(string oldName, string newName, string newDescription)
        {
            UniqueName = newName;
            UniqueDescription = newDescription;
            OriginalName = oldName;
            TreeInfo info = PrefabCollection<TreeInfo>.FindLoaded(newName);
            UniqueInfo = new UniqueTreeInfo(info);
        }

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
