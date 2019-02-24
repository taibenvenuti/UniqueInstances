namespace UniqueInstances
{
    public struct UniqueTreeInstance
    {
        public string UniqueName;
        public uint TreeID;
        public string OriginalName;
        public TreeInstance.Flags Flags;

        public UniqueTreeInstance(uint treeID, UniqueTree uniqueTree)
        {
            UniqueName = uniqueTree.UniqueName;
            TreeID = treeID;
            OriginalName = uniqueTree.OriginalName;
            Flags = TreeInstance.Flags.Created;
        }

        /// <summary>
        /// Must be called after Data has been initialized.
        /// Sets the custom unique TreeInfo to a TreeInstance instance.
        /// </summary>
        public void Load()
        {
            TreeManager.instance.m_trees.m_buffer[TreeID].Info = PrefabCollection<TreeInfo>.FindLoaded(UniqueName);
        }
    }
}
