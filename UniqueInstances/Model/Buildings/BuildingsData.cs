using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueInstances
{
    [Serializable]
    public class BuildingsData : IEnumerable<UniqueBuilding>
    {
        private Dictionary<string, UniqueBuilding> InfoBuffer { get; set; }

        public BuildingsData()
        {
            InfoBuffer = new Dictionary<string, UniqueBuilding>();
        }

        public bool Contains(UniqueBuilding uniqueBuilding)
        {
            return uniqueBuilding != null && InfoBuffer.ContainsKey(uniqueBuilding.UniqueName);
        }

        public void Add(UniqueBuilding uniqueBuilding, bool replace = false)
        {
            string uniqueName = uniqueBuilding.UniqueName;

            if (!InfoBuffer.ContainsKey(uniqueName))
            {
                InfoBuffer.Add(uniqueName, uniqueBuilding);
            }
            else if (replace) InfoBuffer[uniqueName] = uniqueBuilding;
        }

        public void Remove(string uniqueName)
        {
            if (InfoBuffer.ContainsKey(uniqueName))
                InfoBuffer.Remove(uniqueName);
        }

        public UniqueBuilding Get(string uniqueNme)
        {
            return InfoBuffer.ContainsKey(uniqueNme) ? null : InfoBuffer[uniqueNme];
        }

        public IEnumerator<UniqueBuilding> GetEnumerator()
        {
            return InfoBuffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
