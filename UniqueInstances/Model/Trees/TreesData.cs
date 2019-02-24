using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueInstances
{
    [Serializable]
    public class TreesData : IEnumerable<UniqueTree>
    {
        private Dictionary<string, UniqueTree> Buffer { get; set; }

        public TreesData()
        {
            Buffer = new Dictionary<string, UniqueTree>();
        }

        public bool Contains(UniqueTree uniqueTree)
        {
            return uniqueTree != null && Buffer.ContainsKey(uniqueTree.UniqueName);
        }

        public void Add(UniqueTree uniqueTree, bool replace = false)
        {
            string id = uniqueTree.UniqueName;

            if (!Buffer.ContainsKey(id))
            {
                Buffer.Add(id, uniqueTree);
            }
            else if (replace) Buffer[id] = uniqueTree;
        }

        public void Remove(string uniqueName)
        {
            if (Buffer.ContainsKey(uniqueName))
                Buffer.Remove(uniqueName);
        }

        public UniqueTree Get(string uniqueName)
        {
            return Buffer.ContainsKey(uniqueName) ? null : Buffer[uniqueName];
        }

        public IEnumerator<UniqueTree> GetEnumerator()
        {
            return Buffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
