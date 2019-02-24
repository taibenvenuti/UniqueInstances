using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniqueInstances
{
    [Serializable]
    public class BuildingInstancesData : IEnumerable<UniqueBuildingInstance>
    {
        private Dictionary<ushort, UniqueBuildingInstance> Buffer { get; set; }

        public BuildingInstancesData()
        {
            Buffer = new Dictionary<ushort, UniqueBuildingInstance>();
        }

        public bool Contains(ushort buildingID)
        {
            if (Buffer.ContainsKey(buildingID))
                return true;
            return false;
        }

        public void Add(ushort buildingID, UniqueBuilding uniqueBuilding, bool replace = false)
        {
            if (!Buffer.ContainsKey(buildingID))
                Buffer.Add(buildingID, new UniqueBuildingInstance(buildingID, uniqueBuilding));
            else if (replace)
                Buffer[buildingID] = new UniqueBuildingInstance(buildingID, uniqueBuilding);
        }

        public bool Remove(ushort buildingID)
        {
            if (Buffer.Remove(buildingID))
                return true;
            return false;
        }

        public UniqueBuildingInstance Get(ushort buildingID)
        {
            return buildingID == 0 || !Buffer.ContainsKey(buildingID) ? default(UniqueBuildingInstance) : Buffer[buildingID];
        }

        public IEnumerator<UniqueBuildingInstance> GetEnumerator()
        {
            return Buffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
