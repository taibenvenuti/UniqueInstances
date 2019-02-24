using System;

namespace UniqueInstances
{
    [Serializable]
    public struct UniqueBuildingInstance
    {
        public string UniqueName;
        public ushort BuildingID;
        public string OriginalName;
        public Building.Flags Flags;

        public UniqueBuildingInstance(ushort buildingID, UniqueBuilding uniqueBuilding)
        {
            UniqueName = uniqueBuilding.UniqueName;
            BuildingID = buildingID;
            OriginalName = uniqueBuilding.OriginalName;
            Flags = Building.Flags.Created;
        }

        internal void Load()
        {
            BuildingManager.instance.m_buildings.m_buffer[BuildingID].Info = PrefabCollection<BuildingInfo>.FindLoaded(UniqueName);
        }
    }
}
