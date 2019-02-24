using System;
using System.Collections;
using System.Collections.Generic;

namespace UniqueInstances
{
    public class TreeInstancesData : IEnumerable<UniqueTreeInstance>
    {
        private Dictionary<uint, UniqueTreeInstance> Buffer { get; set; }

        public bool Contains(uint treeID)
        {
            if (Buffer.ContainsKey(treeID))
                return true;
            return false;
        }

        public void Add(uint treeID, UniqueTree uniqueTree, bool replace = false)
        {
            if (!Buffer.ContainsKey(treeID))
                Buffer.Add(treeID, new UniqueTreeInstance(treeID, uniqueTree));
            if (replace)
                Buffer[treeID] = new UniqueTreeInstance(treeID, uniqueTree);
        }

        public void Remove(uint treeID)
        {
            if (Buffer.ContainsKey(treeID))
                Buffer.Remove(treeID);
        }

        public UniqueTreeInstance Get(uint treeID)
        {
            return treeID == 0 || !Buffer.ContainsKey(treeID) ? default(UniqueTreeInstance) : Buffer[treeID];
        }

        public IEnumerator<UniqueTreeInstance> GetEnumerator()
        {
            return Buffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
