using System;
using System.Threading;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace UniqueInstances
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        public Data Data { get; set; }
        public InstancesData InstancesData { get; set; }
        private UniqueInstancesTool Tool { get; set; }
        private bool Active { get; set; }
        private uint TreeId { get; set; }
        private object PrefabLock { get; set; }

        void Awake()
        {
            Instance = this;
            PrefabLock = new object();
            Serializer.Load();
            InstallTool();
        }

        void OnDestroy()
        {
            UninstallTool();
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

        internal bool IsUnique(InstanceID instanceID)
        {
            if (instanceID.Building != 0)
                return InstancesData.Buildings.Contains(instanceID.Building);
            if (instanceID.Tree != 0)
                return InstancesData.Trees.Contains(instanceID.Tree);
            return false;
        }

        private void InstallTool()
        {
            Tool = ToolsModifierControl.toolController.gameObject.AddComponent<UniqueInstancesTool>();
            FieldInfo fieldInfo = typeof(ToolController).GetField("m_tools", BindingFlags.Instance | BindingFlags.NonPublic);
            ToolBase[] tools = (ToolBase[])fieldInfo.GetValue(ToolsModifierControl.toolController);
            int initialLength = tools.Length;
            Array.Resize(ref tools, initialLength + 1);
            Dictionary<Type, ToolBase> dictionary = (Dictionary<Type, ToolBase>)typeof(ToolsModifierControl).GetField("m_Tools", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            dictionary.Add(typeof(UniqueInstancesTool), Tool);
            tools[initialLength] = Tool;
            fieldInfo.SetValue(ToolsModifierControl.toolController, tools);
        }

        private void UninstallTool()
        {
            FieldInfo fieldInfo = typeof(ToolController).GetField("m_tools", BindingFlags.Instance | BindingFlags.NonPublic);
            List<ToolBase> tools = ((ToolBase[])fieldInfo.GetValue(ToolsModifierControl.toolController)).ToList();
            tools.Remove(Tool);
            fieldInfo.SetValue(ToolsModifierControl.toolController, tools.ToArray());
            Dictionary<Type, ToolBase> dictionary = (Dictionary<Type, ToolBase>)typeof(ToolsModifierControl).GetField("m_Tools", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            dictionary.Remove(typeof(UniqueInstancesTool));
            Destroy(ToolsModifierControl.toolController.gameObject.GetComponent<UniqueInstancesTool>());
        }


        public void InitializeData()
        {
            while (!Monitor.TryEnter(PrefabLock, SimulationManager.SYNCHRONIZE_TIMEOUT)) { }
            try
            {
                InitializeBuildings();
                InitializeTrees();
            }
            finally
            {
                Monitor.Exit(PrefabLock);
            }
        }

        private void InitializeBuildings()
        {
            //Create the new infos
            foreach (var uniqueBuilding in Data.Buildings)
            {
                uniqueBuilding.CreateCopy();
            }

            //Bind new copies to prefab collection
            PrefabCollection<BuildingInfo>.BindPrefabs();

            //Copy fields from old info to new info, then overwrite with saved custom values
            foreach (var uniqueBuilding in Data.Buildings)
            {
                uniqueBuilding.CopyFields();
                uniqueBuilding.Load();
            }

            //Set new infos on buildin instances
            foreach (var buildingInstance  in InstancesData.Buildings)
            {
                if ((buildingInstance.Flags & Building.Flags.Created) != 0)
                {
                    buildingInstance.Load();
                }
            }
        }

        private void InitializeTrees()
        {
            //Create the new infos
            foreach (var uniqueTree in Data.Trees)
            {
                uniqueTree.CreateCopy();
            }

            //Bind new copies to prefab collection
            PrefabCollection<TreeInfo>.BindPrefabs();

            //Copy fields from old info to new info, then overwrite with saved custom values
            foreach (var uniqueTree in Data.Trees)
            {
                uniqueTree.CopyFields();
                uniqueTree.Load();
            }

            //Set new infos on buildin instances
            foreach (var treeInstance in InstancesData.Trees)
            {
                if ((treeInstance.Flags & TreeInstance.Flags.Created) != 0)
                {
                    treeInstance.Load();
                }
            }
        }
    }
}
