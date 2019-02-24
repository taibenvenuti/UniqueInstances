using System;
using System.Threading;
using UnityEngine;
using UniqueInstances.Data;

namespace UniqueInstances.Controller
{
    public class UniqueController : MonoBehaviour
    {
        public static UniqueController Instance;
        public UniqueData Data { get; set; }
        public UniqueSaveGameData SaveGameData { get; set; }
        private UniqueTool Tool { get; set; }
        private bool Active { get; set; }
        private uint TreeId { get; set; }
        private object PrefabLock { get; set; }

        void Awake()
        {
            PrefabLock = new object();
            Data = gameObject.AddComponent<UniqueData>();
            Tool = ToolsModifierControl.toolController.gameObject.AddComponent<UniqueTool>();
            Tool.Initialize(this);
        }

        internal void MakeUnique(InstanceID hoverInstance, bool v)
        {
            while (!Monitor.TryEnter(PrefabLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                if (!Active)
                {
                    if (hoverInstance.Building != 0)
                        MakeUnique(hoverInstance.Building, v);
                    else if (hoverInstance.Tree != 0)
                        MakeUnique(hoverInstance.Tree, v);
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

        private void MakeUnique(ushort buildingID, bool state)
        {

        }

        private void MakeUnique(uint treeID, bool state)
        {

        }
    }
}
