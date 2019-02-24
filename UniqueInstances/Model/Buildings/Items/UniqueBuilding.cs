using ColossalFramework.Math;
using System;

namespace UniqueInstances
{
    [Serializable]
    public class UniqueBuilding
    {
        public string UniqueName { get; set; }
        public string UniqueDescription { get; set; }
        public string OriginalName { get; private set; }
        public UniqueBuildingInfo UniqueInfo { get; private set; }
        public Randomizer Randomizer { get; set; }

        public UniqueBuilding(BuildingInfo newInfo, string oldName, string newName, string newDescription)
        {
            UniqueName = newName;
            UniqueDescription = newDescription;
            OriginalName = oldName;
            UniqueInfo = new UniqueBuildingInfo(newInfo);
        }

        /// <summary>
        /// Must be called after Data has been initialized.
        /// Sets saved custom 'properties' to a unique BuildingInfo.
        /// </summary>
        public void Load()
        {
            UniqueInfo.Load(PrefabCollection<BuildingInfo>.FindLoaded(UniqueName));
        }

        public void CreateCopy()
        {
            PrefabCollection<BuildingInfo>.FindLoaded(OriginalName).CopyInfo(UniqueName);
        }

        public void CopyFields()
        {
            PrefabCollection<BuildingInfo>.FindLoaded(OriginalName).CopyFields(UniqueName);
        }
    }
}
