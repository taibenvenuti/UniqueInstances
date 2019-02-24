using System;

namespace UniqueInstances
{
    [Serializable]
    public class Data
    {
        public const ushort MAX_UNIQUE_COUNT = ushort.MaxValue;

        public TreesData Trees { get; set; }

        public BuildingsData Buildings { get; set; }
    }
}
