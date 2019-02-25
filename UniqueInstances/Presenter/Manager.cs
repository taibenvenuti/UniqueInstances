using System;
using System.Threading;
using UnityEngine;

namespace UniqueInstances
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        public Data Data { get; set; }
        private bool Active { get; set; }
        private object PrefabLock { get; set; }

        void Awake()
        {
            Instance = this;
            PrefabLock = new object();
            UniqueTool.Create();
            ByteSerializer.Load();
            InitializeData();
        }

        void OnDestroy()
        {
            UniqueTool.Destroy();
        }
        //TODO Name/Desc input UI
        internal void MakeUnique(InstanceID hoverInstance, bool create)
        {
            while (!Monitor.TryEnter(PrefabLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                if (!Active)
                {
                    if (hoverInstance.Building != 0)
                        MakeUnique(hoverInstance.Building, "TEST_NAME", "TEST_DESCRIPTION", create);
                    else if (hoverInstance.Tree != 0)
                        MakeUnique(hoverInstance.Tree, "TEST_NAME", "TEST_DESCRIPTION", create);
                    Active = true;
                } 
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
            finally
            {
                Monitor.Exit(PrefabLock);
                Active = false;
            }
        }

        private void MakeUnique(ushort buildingID, string newName, string newDescription = "", bool create = true)
        {
            if (create)
                Data.CreateUniqueInstance(buildingID, newName, newDescription);
            else Data.RemoveInstance(buildingID);
        }

        private void MakeUnique(uint treeID, string newName, string newDescription = "", bool create = true)
        {
            if (create)
                Data.CreateUniqueInstance(treeID, newName, newDescription);
            else Data.RemoveInstance(treeID);
        }

        public bool IsUnique(InstanceID instanceID)
        {
            if (instanceID.Building != 0)
                return Data.ContainsInstance(instanceID.Building);
            if (instanceID.Tree != 0)
                return Data.ContainsInstance(instanceID.Tree);
            return false;
        }

        public void InitializeData()
        {
            while (!Monitor.TryEnter(PrefabLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                Data.InitializeBuildings();
                Data.InitializeTrees();
            }
            finally
            {
                Monitor.Exit(PrefabLock);
            }
        }
    }
}
