using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueInstances
{
    [Serializable]
    public class TreesData : IEnumerable<UniqueTree>
    {
        private Dictionary<ulong, UniqueTree> Buffer { get; set; }

        private TreesData()
        {
            Buffer = new Dictionary<ulong, UniqueTree>();
        }

        public bool Contains(UniqueTree uniqueTree)
        {
            return uniqueTree != null && uniqueTree.UniqueID != 0 && Buffer.ContainsKey(uniqueTree.UniqueID);
        }

        public void Add(UniqueTree uniqueTree, bool replace = false)
        {
            ulong id = uniqueTree.UniqueID;

            if (!Buffer.ContainsKey(id))
            {
                Buffer.Add(id, uniqueTree);
            }
            if (replace) Buffer[id] = uniqueTree;
        }

        public void Remove(UniqueTree uniqueTree)
        {
            ulong id = uniqueTree.UniqueID;

            if (Buffer.ContainsKey(id))
                Buffer.Remove(id);
        }

        public UniqueTree Get(ulong uniqueID)
        {
            return uniqueID == 0 || Buffer.ContainsKey(uniqueID) ? null : Buffer[uniqueID];
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
