using ColossalFramework;
using ColossalFramework.Math;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UniqueInstances.Controller
{
    public class UniqueTool : ToolBase
    {
        private Ray MouseRay { get; set; }
        private float MouseRayLength { get; set; }
        private Vector3 MouseRayRight { get; set; }
        private bool MouseLeftDown { get; set; }
        private InstanceID HoverInstance { get; set; }
        private bool MouseRightDown { get; set; }
        private Vector3 CachedPosition { get; set; }
        private UniqueController Controller { get; set; }
        private Randomizer Randomizer { get; set; }
        private Vector3 MousePosition { get; set; }
        private bool MouseRayValid { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Randomizer = new Randomizer((int)DateTime.Now.Ticks);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //this.m_toolController.ClearColliding();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MouseLeftDown = false;
            MouseRightDown = false;
            MouseRayValid = false;
        }

        public void Initialize(UniqueController controller)
        {
            Controller = controller;
            
            var fieldInfo = ToolsModifierControl.toolController.GetType().GetField("m_tools", BindingFlags.Instance | BindingFlags.NonPublic);
            var tools = (ToolBase[])fieldInfo.GetValue(ToolsModifierControl.toolController);
            var initialLength = tools.Length;
            Array.Resize(ref tools, initialLength + 1);
            var dictionary = (Dictionary<Type, ToolBase>)typeof(ToolsModifierControl).GetField("m_Tools", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            dictionary.Add(typeof(UniqueTool), this);
            tools[initialLength] = this;
            fieldInfo.SetValue(ToolsModifierControl.toolController, tools);
        }

        public override void SimulationStep()
        {
            ToolBase.RaycastInput input = new ToolBase.RaycastInput(MouseRay, MouseRayLength);
            input.m_rayRight = MouseRayRight;
            bool rayCast = ToolBase.RayCast(input, out RaycastOutput output);
            MousePosition = output.m_hitPos;
            InstanceID instanceID = InstanceID.Empty;

            if (output.m_building != 0)
                instanceID.Building = output.m_building;
            else if (output.m_treeInstance != 0)
                instanceID.Tree = output.m_treeInstance;
            SetHoverInstance(instanceID);

            if (rayCast && MouseRayValid)
            {
                if (MouseLeftDown != MouseRightDown)
                {
                    ApplyTool();
                }
            }
        }

        private void ApplyTool()
        {
            if (MouseLeftDown)
                Controller.MakeUnique(HoverInstance, true);
            else if (MouseRightDown)
                Controller.MakeUnique(HoverInstance, false);
        }

        protected override void OnToolGUI(Event e)
        {
            if (!this.m_toolController.IsInsideUI && e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    MouseLeftDown = true;
                }
                else if (e.button == 1)
                {
                    MouseRightDown = true;
                }
            }
            else if (e.type == EventType.MouseUp)
            {
                if (e.button == 0)
                {
                    MouseLeftDown = false;
                }
                else if (e.button == 1)
                {
                    MouseRightDown = false;
                }
            }
        }

        protected override void OnToolUpdate()
        {
            if (!m_toolController.IsInsideUI && Cursor.visible)
            {
                bool show = false;
                string text = string.Empty;
                if (HoverInstance.Building != 0)
                {
                    show = true;
                    text = "Test building message.";
                }
                else if (HoverInstance.Tree != 0)
                {
                    show = true;
                    text = "Test tree message.";
                }
                if (show)
                {
                    base.ShowExtraInfo(true, text, CachedPosition);
                }
                else base.ShowExtraInfo(false, null, CachedPosition);
            }
            else base.ShowExtraInfo(false, null, CachedPosition);
        }

        protected override void OnToolLateUpdate()
        {
            MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            MouseRayLength = Camera.main.farClipPlane;
            MouseRayRight = Camera.main.transform.TransformDirection(Vector3.right);
            MouseRayValid = (!m_toolController.IsInsideUI && Cursor.visible);
            CachedPosition = MousePosition;
        }

        private void SetHoverInstance(InstanceID id)
        {
            if (id != HoverInstance)
            {
                if (HoverInstance.Tree != 0u)
                {
                    if (Singleton<TreeManager>.instance.m_trees.m_buffer[(int)((UIntPtr)HoverInstance.Tree)].Hidden)
                    {
                        Singleton<TreeManager>.instance.m_trees.m_buffer[(int)((UIntPtr)HoverInstance.Tree)].Hidden = false;
                        Singleton<TreeManager>.instance.UpdateTreeRenderer(HoverInstance.Tree, true);
                    }
                }
                else if (HoverInstance.Building != 0)
                {
                    if ((Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)HoverInstance.Building].m_flags & Building.Flags.Hidden) != Building.Flags.None)
                    {
                        Building[] buffer2 = Singleton<BuildingManager>.instance.m_buildings.m_buffer;
                        ushort building = HoverInstance.Building;
                        buffer2[building].m_flags = (buffer2[building].m_flags & ~Building.Flags.Hidden);
                        Singleton<BuildingManager>.instance.UpdateBuildingRenderer(HoverInstance.Building, true);
                    }
                }
                HoverInstance = id;
            }
        }
    }
}
