using System;

namespace UniqueInstances
{
    [Serializable]
    public class UniqueBuildingInfo
    {
        public BuildingAI m_buildingAI;
        public BuildingInfo.Prop[] m_props;
        public BuildingInfo.PlacementMode m_placementMode;
        public bool m_flattenTerrain;
        public bool m_fullGravel;
        public bool m_fullPavement;

        public UniqueBuildingInfo(BuildingInfo oldInfo)
        {
            m_buildingAI = oldInfo.m_buildingAI;
            m_props = oldInfo.m_props;
            m_placementMode = oldInfo.m_placementMode;
            m_flattenTerrain = oldInfo.m_flattenTerrain;
            m_fullGravel = oldInfo.m_fullGravel;
            m_fullPavement = oldInfo.m_fullPavement;
        }

        public void Load(BuildingInfo newInfo)
        {
            newInfo.m_buildingAI = m_buildingAI;
            newInfo.m_props = m_props;
            newInfo.m_placementMode = m_placementMode;
            newInfo.m_flattenTerrain = m_flattenTerrain;
            newInfo.m_fullGravel = m_fullGravel;
            newInfo.m_fullPavement = m_fullPavement;
        }
    }
}
