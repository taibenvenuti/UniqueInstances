using ColossalFramework;
using ColossalFramework.Math;
using System;
using UnityEngine;

namespace UniqueInstances
{
    public class UniqueInstancesTool : ToolBase
    {
        private Ray MouseRay { get; set; }
        private float MouseRayLength { get; set; }
        private Vector3 MouseRayRight { get; set; }
        private bool MouseLeftDown { get; set; }
        private InstanceID HoverInstance { get; set; }
        private bool MouseRightDown { get; set; }
        private Vector3 CachedPosition { get; set; }
        private Randomizer Randomizer { get; set; }
        private Vector3 MousePosition { get; set; }
        private bool MouseRayValid { get; set; }
        private RaycastOutput RayOutput { get; set; }
        public Color32 UniqueColor { get; set; } = new Color32(128, 0, 0,255);
        public Color32 NormalColor { get; set; } = new Color32(0, 128, 0, 255);

        protected override void Awake()
        {
            base.Awake();

            Randomizer = new Randomizer((int)DateTime.Now.Ticks);
            Debug.Log("Tool ready...");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MouseLeftDown = false;
            MouseRightDown = false;
            MouseRayValid = false;
        }

        public override void SimulationStep()
        {
            Debug.LogWarning("SimulationStep");

            ToolBase.RaycastInput input = new ToolBase.RaycastInput(MouseRay, MouseRayLength);

            input.m_rayRight = MouseRayRight;

            if (ToolBase.RayCast(input, out ToolBase.RaycastOutput RayOutput))
            {
                MousePosition = RayOutput.m_hitPos;

                InstanceID instanceID = InstanceID.Empty;

                if (RayOutput.m_building != 0 && (Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)RayOutput.m_building].m_flags & Building.Flags.Untouchable) != Building.Flags.None)
                {
                    RayOutput.m_building = Building.FindParentBuilding(RayOutput.m_building);
                }
                if (RayOutput.m_building != 0)
                {
                    RayOutput.m_hitPos = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)RayOutput.m_building].m_position;
                }
                else if (RayOutput.m_treeInstance != 0u)
                {
                    RayOutput.m_hitPos = Singleton<TreeManager>.instance.m_trees.m_buffer[(int)((UIntPtr)RayOutput.m_treeInstance)].Position;
                }

                if (RayOutput.m_building != 0)
                    instanceID.Building = RayOutput.m_building;
                else if (RayOutput.m_treeInstance != 0)
                    instanceID.Tree = RayOutput.m_treeInstance;

                SetHoverInstance(instanceID);

                if (MouseLeftDown != MouseRightDown)
                {
                    ApplyTool();
                }
            }
        }

        private void ApplyTool()
        {
            if (!MouseRayValid) return;

            if (MouseLeftDown)
                Manager.Instance.MakeUnique(HoverInstance, true);
            else if (MouseRightDown)
                Manager.Instance.MakeUnique(HoverInstance, false);
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
            if (MouseRayValid)
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

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo)
        {
            if (this.m_toolController.IsInsideUI || !Cursor.visible)
            {
                base.RenderOverlay(cameraInfo);
                return;
            }
            switch (HoverInstance.Type)
            {
                case InstanceType.Building:
                    {
                        ushort building = HoverInstance.Building;
                        NetManager instance = Singleton<NetManager>.instance;
                        BuildingManager instance2 = Singleton<BuildingManager>.instance;
                        BuildingInfo info5 = instance2.m_buildings.m_buffer[(int)building].Info;
                        Color toolColor6 = GetOverlayColor();
                        float num = 1f;
                        BuildingTool.CheckOverlayAlpha(info5, ref num);
                        ushort num2 = instance2.m_buildings.m_buffer[(int)building].m_netNode;
                        int num3 = 0;
                        while (num2 != 0)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                ushort segment = instance.m_nodes.m_buffer[(int)num2].GetSegment(j);
                                if (segment != 0 && instance.m_segments.m_buffer[(int)segment].m_startNode == num2 && (instance.m_segments.m_buffer[(int)segment].m_flags & NetSegment.Flags.Untouchable) != NetSegment.Flags.None)
                                {
                                    NetTool.CheckOverlayAlpha(ref instance.m_segments.m_buffer[(int)segment], ref num);
                                }
                            }
                            num2 = instance.m_nodes.m_buffer[(int)num2].m_nextBuildingNode;
                            if (++num3 > 32768)
                            {
                                CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                                break;
                            }
                        }
                        ushort subBuilding = instance2.m_buildings.m_buffer[(int)building].m_subBuilding;
                        num3 = 0;
                        while (subBuilding != 0)
                        {
                            BuildingTool.CheckOverlayAlpha(instance2.m_buildings.m_buffer[(int)subBuilding].Info, ref num);
                            subBuilding = instance2.m_buildings.m_buffer[(int)subBuilding].m_subBuilding;
                            if (++num3 > 49152)
                            {
                                CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                                break;
                            }
                        }
                        toolColor6.a *= num;
                        int length = instance2.m_buildings.m_buffer[(int)building].Length;
                        Vector3 position2 = instance2.m_buildings.m_buffer[(int)building].m_position;
                        float angle2 = instance2.m_buildings.m_buffer[(int)building].m_angle;
                        BuildingTool.RenderOverlay(cameraInfo, info5, length, position2, angle2, toolColor6, false);
                        num2 = instance2.m_buildings.m_buffer[(int)building].m_netNode;
                        num3 = 0;
                        while (num2 != 0)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                ushort segment2 = instance.m_nodes.m_buffer[(int)num2].GetSegment(k);
                                if (segment2 != 0 && instance.m_segments.m_buffer[(int)segment2].m_startNode == num2 && (instance.m_segments.m_buffer[(int)segment2].m_flags & NetSegment.Flags.Untouchable) != NetSegment.Flags.None)
                                {
                                    NetTool.RenderOverlay(cameraInfo, ref instance.m_segments.m_buffer[(int)segment2], toolColor6, toolColor6);
                                }
                            }
                            num2 = instance.m_nodes.m_buffer[(int)num2].m_nextBuildingNode;
                            if (++num3 > 32768)
                            {
                                CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                                break;
                            }
                        }
                        subBuilding = instance2.m_buildings.m_buffer[(int)building].m_subBuilding;
                        num3 = 0;
                        while (subBuilding != 0)
                        {
                            BuildingInfo info6 = instance2.m_buildings.m_buffer[(int)subBuilding].Info;
                            int length2 = instance2.m_buildings.m_buffer[(int)subBuilding].Length;
                            Vector3 position3 = instance2.m_buildings.m_buffer[(int)subBuilding].m_position;
                            float angle3 = instance2.m_buildings.m_buffer[(int)subBuilding].m_angle;
                            BuildingTool.RenderOverlay(cameraInfo, info6, length2, position3, angle3, toolColor6, false);
                            subBuilding = instance2.m_buildings.m_buffer[(int)subBuilding].m_subBuilding;
                            if (++num3 > 49152)
                            {
                                CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                                break;
                            }
                        }
                        break;
                    }
                case InstanceType.Tree:
                    {
                        uint tree = HoverInstance.Tree;
                        TreeManager instance11 = Singleton<TreeManager>.instance;
                        TreeInfo info9 = instance11.m_trees.m_buffer[(int)((UIntPtr)tree)].Info;
                        Vector3 position6 = instance11.m_trees.m_buffer[(int)((UIntPtr)tree)].Position;
                        Randomizer randomizer4 = new Randomizer(tree);
                        float scale4 = info9.m_minScale + (float)randomizer4.Int32(10000u) * (info9.m_maxScale - info9.m_minScale) * 0.0001f;
                        Color toolColor13 = GetOverlayColor();
                        float num13 = 1f;
                        TreeTool.CheckOverlayAlpha(info9, scale4, ref num13);
                        toolColor13.a *= num13;
                        TreeTool.RenderOverlay(cameraInfo, info9, position6, scale4, toolColor13);
                        break;
                    }
            }
            base.RenderOverlay(cameraInfo);
        }

        private Color GetOverlayColor()
        {
            return Manager.Instance.IsUnique(HoverInstance) ? UniqueColor : NormalColor;
        }
    }
}
