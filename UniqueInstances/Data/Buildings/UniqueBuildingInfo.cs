using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniqueInstances.Data.Buildings
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

        public UniqueBuildingInfo(BuildingInfo info)
        {
            m_buildingAI = info.m_buildingAI;
            m_props = info.m_props;
            m_placementMode = info.m_placementMode;
            m_flattenTerrain = info.m_flattenTerrain;
            m_fullGravel = info.m_fullGravel;
            m_fullPavement = info.m_fullPavement;
        }
    }
}
