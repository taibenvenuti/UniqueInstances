using System;

namespace UniqueInstances
{
    [Serializable]
    public class Data
    {
        public const ushort MAX_UNIQUE_COUNT = ushort.MaxValue;

        private TreesData TreeInfos { get; set; }

        private TreeInstancesData TreeInstances { get; set; }

        private BuildingsData BuildingInfos { get; set; }

        private BuildingInstancesData BuildingInstances { get; set; }

        public Data()
        {
            TreeInfos = new TreesData();
            BuildingInfos = new BuildingsData();
            TreeInstances = new TreeInstancesData();
            BuildingInstances = new BuildingInstancesData();
        }

        /// <summary>
        /// Initializes the Data objects.
        /// Creates unique copies of BuildingInfos from saved data,
        /// then loads them into specific instances that use them.
        /// </summary>
        public void InitializeBuildings()
        {
            //Create the new infos
            foreach (var uniqueBuilding in BuildingInfos)
            {
                uniqueBuilding.CreateCopy();
            }

            //Bind new copies to prefab collection
            PrefabCollection<BuildingInfo>.BindPrefabs();

            //Copy fields from old info to new info, then overwrite with saved custom values
            foreach (var uniqueBuilding in BuildingInfos)
            {
                uniqueBuilding.CopyFields();
                uniqueBuilding.Load();
            }

            //Set new infos on buildin instances
            foreach (var buildingInstance in BuildingInstances)
            {
                if ((buildingInstance.Flags & Building.Flags.Created) != 0)
                {
                    buildingInstance.Load();
                }
            }
        }

        /// <summary>
        /// Initializes the Data objects.
        /// Creates unique copies of TreeInfos from saved data,
        /// then loads them into specific instances that use them.
        /// </summary>
        public void InitializeTrees()
        {
            //Create the new infos
            foreach (var uniqueTree in TreeInfos)
            {
                uniqueTree.CreateCopy();
            }

            //Bind new copies to prefab collection
            PrefabCollection<TreeInfo>.BindPrefabs();

            //Copy fields from old info to new info, then overwrite with saved custom values
            foreach (var uniqueTree in TreeInfos)
            {
                uniqueTree.CopyFields();
                uniqueTree.Load();
            }

            //Set new infos on building instances
            foreach (var treeInstance in TreeInstances)
            {
                if ((treeInstance.Flags & TreeInstance.Flags.Created) != 0)
                {
                    treeInstance.Load();
                }
            }
        }
        
        public void CreateUniqueInstance(ushort buildingID, string newName, string newDescription = "")
        {
            BuildingInfo oldInfo = BuildingManager.instance.m_buildings.m_buffer[buildingID].Info;
            UniqueBuilding uniqueBuilding = oldInfo.GetImmediateCopy(newName);
            BuildingInfos.Add(uniqueBuilding);
            BuildingInstances.Add(buildingID, uniqueBuilding);
        }

        public void CreateUniqueInstance(uint treeID, string newName, string newDescription = "")
        {
            TreeInfo oldInfo = TreeManager.instance.m_trees.m_buffer[treeID].Info;
            UniqueTree uniqueTree = oldInfo.GetImmediateCopy(newName);
            TreeInfos.Add(uniqueTree);
            TreeInstances.Add(treeID, uniqueTree);
        }

        public bool RemoveInstance(ushort buildingID, bool removeInfo = true)
        {
            UniqueBuildingInstance instance = BuildingInstances.Get(buildingID);
            if ((instance.Flags & Building.Flags.Created) != Building.Flags.None)
            {
                if (removeInfo)
                    BuildingInfos.Remove(instance.UniqueName);
                BuildingInstances.Remove(buildingID);
                return true;
            }
            return false;
        }

        public bool RemoveInstance(uint treeID, bool removeInfo = true)
        {
            UniqueTreeInstance instance = TreeInstances.Get(treeID);
            if ((instance.Flags & TreeInstance.Flags.Created) != TreeInstance.Flags.None)
            {
                if (removeInfo)
                    TreeInfos.Remove(instance.UniqueName);
                TreeInstances.Remove(treeID);
                return true;
            }
            return false;
        }

        public bool ContainsInstance(ushort buildingID)
        {
            return BuildingInstances.Contains(buildingID);
        }

        public bool ContainsInstance(uint treeID)
        {
            return TreeInstances.Contains(treeID);
        }

        public SerializableInstances GetSerializableInstances()
        {
            return new SerializableInstances(this);
        }

        public SerializableInfos GetSerializableInfos()
        {
            return new SerializableInfos(this);
        }

        public void LoadDeserializedInstances(SerializableInstances data)
        {
            TreeInstances = data.TreeInstances;
            BuildingInstances = data.BuildingInstances;
        }

        public void LoadDeserializedInfos(SerializableInfos data)
        {
            TreeInfos = data.TreeInfos;
            BuildingInfos = data.BuildingInfos;
        }

        public class SerializableInstances
        {
            public TreeInstancesData TreeInstances;
            public BuildingInstancesData BuildingInstances;

            public SerializableInstances(Data data)
            {
                TreeInstances = data.TreeInstances;
                BuildingInstances = data.BuildingInstances;
            }
        }

        public class SerializableInfos
        {
            public TreesData TreeInfos;
            public BuildingsData BuildingInfos;

            public SerializableInfos(Data data)
            {
                TreeInfos = data.TreeInfos;
                BuildingInfos = data.BuildingInfos;
            }
        }
    }
}
