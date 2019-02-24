using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueInstances
{
    [Serializable]
    public class BuildingsData : IEnumerable<UniqueBuilding>
    {
        private Dictionary<string, UniqueBuilding> Buffer { get; set; }

        public BuildingsData()
        {
            Buffer = new Dictionary<string, UniqueBuilding>();
        }

        public bool Contains(UniqueBuilding uniqueBuilding)
        {
            return uniqueBuilding != null && Buffer.ContainsKey(uniqueBuilding.UniqueName);
        }

        public void Add(UniqueBuilding uniqueBuilding, bool replace = false)
        {
            string id = uniqueBuilding.UniqueName;

            if (!Buffer.ContainsKey(id))
            {
                Buffer.Add(id, uniqueBuilding);
            }
            if (replace) Buffer[id] = uniqueBuilding;
        }

        public void Remove(UniqueBuilding uniqueBuilding)
        {
            string id = uniqueBuilding.UniqueName;

            if (Buffer.ContainsKey(id))
                Buffer.Remove(id);
        }

        public UniqueBuilding Get(string uniqueNme)
        {
            return Buffer.ContainsKey(uniqueNme) ? null : Buffer[uniqueNme];
        }

        public IEnumerator<UniqueBuilding> GetEnumerator()
        {
            return Buffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
