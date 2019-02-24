using System;

namespace UniqueInstances
{
    [Serializable]
    public class InstancesData
    {
        public TreeInstancesData Trees { get; set; }

        public BuildingInstancesData Buildings { get; set; }
    }
}
