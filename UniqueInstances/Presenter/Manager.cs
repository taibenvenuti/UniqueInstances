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
        private UniqueInstancesTool Tool { get; set; }
        private bool Active { get; set; }
        private object PrefabLock { get; set; }

        void Awake()
        {
            Instance = this;
            PrefabLock = new object();
            ByteSerializer.Load();
            InstallTool();
            InitializeData();
        }

        void OnDestroy()
        {
            UninstallTool();
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
