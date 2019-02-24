using ColossalFramework.Math;
using System;
using UniqueInstances.Extensions;

namespace UniqueInstances.Data.Buildings
{
    [Serializable]
    public struct UniqueBuilding
    {
        public ulong UniqueID { get; private set; }
        public string UniqueName { get; set; }
        public string UniqueDescription { get; set; }
        public string OriginalName { get; private set; }
        public UniqueBuildingInfo UniqueInfo { get; private set; }
        public Randomizer Randomizer { get; set; }

        public int PrefabDataIndex
        {
            get
            {
                if(string.IsNullOrEmpty(UniqueName))
                {
                    BuildingInfo original = PrefabCollection<BuildingInfo>.FindLoaded(OriginalName);
                    if (original == null) return -1;
                    return original.m_prefabDataIndex;
                }
                BuildingInfo copy = PrefabCollection<BuildingInfo>.FindLoaded(UniqueName);
                if (copy == null) return -1;
                return copy.m_prefabDataIndex;
            }
        }

        public UniqueBuilding(ushort buildingID, string newName, string newDescription, bool replace)
        {
            UniqueID = (ulong)DateTime.Now.Ticks + buildingID;
            UniqueName = newName;
            UniqueDescription = newDescription;

            BuildingInfo oldInfo = BuildingManager.instance.m_buildings.m_buffer[buildingID].Info;
            BuildingInfo newInfo = BuildingManager.instance.m_buildings.m_buffer[buildingID].GetCopyOf(newName);

            OriginalName = oldInfo.name;
            UniqueInfo = new UniqueBuildingInfo(newInfo);
            Randomizer = new Randomizer(UniqueID);

            if(replace)
            {
                BuildingManager.instance.m_buildings.m_buffer[buildingID].Info = newInfo;
                BuildingManager.instance.UpdateBuildingRenderer(buildingID, true);
                BuildingManager.instance.UpdateBuildingInfo(buildingID, newInfo);
            }
        }
    }
}
