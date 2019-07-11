using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Color = UnityEngine.Color;
using GUI = UnityEngine.GUI;
using Mathf = UnityEngine.Mathf;
using Rect = UnityEngine.Rect;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Canvas : CanvasObject
    {

        public float Scale { get; private set; } = 1.0f;
        public Vector2 Offset { get; private set; } = Vector2.zero;

        public Matrix4x4 Transform
        {
            get
            {
                return Matrix4x4.TRS((Vector3)(Offset + Vector2.up * EasyGraphWindow.titleHeight), Quaternion.identity, Vector3.one * Scale);
            }
        }
        public Rect Position
        {
            get
            {
                return new Rect(Offset, Size);
            }
        }



        public static readonly Vector2 Size = new Vector2(1000, 500);
        public static readonly Vector2 CanvasScaleRange = new Vector2(0.2f, 5f);
        private static readonly float baseGridSpacing = 20;
        private static readonly int subGridFactor = 5;
        private static readonly Color colorLight = new Color(0.4f, 0.4f, 0.4f, 1f);
        private static readonly Color colorDark = new Color(0.6f, 0.6f, 0.6f, 1f);

        // SortedDictionary 不能逆序遍历
        private readonly Dictionary<int, List<CanvasElement>> managedElements = new Dictionary<int, List<CanvasElement>>();
        private readonly List<int> managedLayers = new List<int>();


        protected override void OnDraw()
        {
            GUI.Box(Position, string.Empty, "box");
            DrawGrids();

            if (Event.current.type == EventType.Repaint)
            {
                foreach (var layer in managedLayers)
                {
                    if (managedElements.TryGetValue(layer, out List<CanvasElement> elementList))
                    {
                        foreach (var ele in elementList)
                        {
                            Draw(ele);
                        }
                    }
                }
            }
        }

        public bool HandleGUIEvent()
        {
            Event current = Event.current;

            bool handled = false;

            for (int i = managedLayers.Count - 1; i >= 0; --i)
            {
                int layer = managedLayers[i];
                if (managedElements.TryGetValue(layer, out List<CanvasElement> elementList))
                {
                    foreach (var ele in elementList)
                    {
                        handled = ele.HandleGUIEvent(current);
                        if (handled)
                        {
                            return true;
                        }
                    }
                }
            }
            return HandleGUIEvent(current);
        }

        protected override bool OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            Offset += delta;
            CheckBounds();
            return true;
        }

        protected override bool OnScroll(Vector2 pos, float delta)
        {

            float newScale = Scale - delta * 0.05f;
            newScale = Mathf.Clamp(newScale, CanvasScaleRange.x, CanvasScaleRange.y);

            // pos 是scale过的 window坐标系下的点坐标


            if (Mathf.Abs(newScale - Scale) > float.Epsilon)
            {

                Vector2 posInCanvas = pos - Offset;
                Vector2 posToWindow = pos * Scale;

                Vector2 newPos = posToWindow / newScale;
                Vector2 newOffset = newPos - posInCanvas;

                Scale = newScale;
                Offset = newOffset;

                CheckBounds();
            }
            return true;
        }

        protected override bool OnContextClick(Vector2 pos)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create Node"), false, () => this.AddElement(new Node(pos - Offset)));
            //genericMenu.DropDown(new Rect(pos + Vector2.up * EasyGraphWindow.titleHeight * (1 - Scale), Vector2.zero));
            genericMenu.ShowAsContext();
            return true;
        }

        private void CheckBounds()
        {
            Vector2 winSize = WindowClip.size;
            float x = Mathf.Clamp(Offset.x, -Size.x, winSize.x);
            float y = Mathf.Clamp(Offset.y, -Size.y, winSize.y);
            Offset = new Vector2(x, y);
        }



        public void Begin()
        {
            Rect windowRect = new Rect(Vector2.zero, GUIUtils.WindowPosition.size);

            GUI.EndGroup();

            Rect windowClip = RectUtils.ScaleRect(windowRect, 1.0f / Scale);
            windowClip = RectUtils.OffsetRect(windowClip,Vector2.up * EasyGraphWindow.titleHeight);
            WindowClip = windowClip;

            GUI.BeginGroup(WindowClip);

            // local2world
            Matrix4x4 transMat = Matrix4x4.TRS(WindowClip.min, Quaternion.identity, Vector3.one);

            // scale
            Vector3 scale = new Vector3(Scale, Scale, 1);
            Matrix4x4 scaleMat = Matrix4x4.Scale(scale);

            // world2local --> scale --> local2world
            Matrix4x4 mat = transMat * scaleMat * transMat.inverse * GUI.matrix;

            GUIUtils.PushGUIMatrix(mat);
        }

        public void End()
        {
            GUIUtils.PopGUIMatrix();
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, EasyGraphWindow.titleHeight, Screen.width, Screen.height));
        }


        private void DrawGrids()
        {
            Handles.BeginGUI();
            GUIUtils.PushHandlesColor(colorDark);
            Rect rect = GUIUtils.WindowPosition;
            int xGrid = Mathf.RoundToInt(Size.x / baseGridSpacing);
            int yGrid = Mathf.RoundToInt(Size.y / baseGridSpacing);

            for (int x = 1; x < xGrid; ++x)
            {
                bool thick = (x % subGridFactor == 0);
                if (thick)
                {
                    GUIUtils.PushHandlesColor(colorLight);
                }
                Vector3 start = Vector3.right * baseGridSpacing * x + (Vector3)Offset;
                Vector3 end = start + Vector3.up * Size.y;
                Handles.DrawLine(start, end);
                if (thick)
                {
                    GUIUtils.PopHandlesColor();
                }
            }
            for (int y = 1; y < yGrid; ++y)
            {
                bool thick = (y % subGridFactor == 0);
                if (thick)
                {
                    GUIUtils.PushHandlesColor(colorLight);
                }
                Vector3 start = Vector3.up * y * baseGridSpacing + (Vector3)Offset;
                Vector3 end = start + Vector3.right * Size.x;
                Handles.DrawLine(start, end);
                if (thick)
                {
                    GUIUtils.PopHandlesColor();
                }
            }

            GUIUtils.PopHandlesColor();
            Handles.EndGUI();
        }

        public Rect WindowClip { get; private set; }


        public void AddElement(CanvasElement canvasElement)
        {
            int layer = canvasElement.Layer;
            List<CanvasElement> elementList = null;
            if (!managedElements.TryGetValue(layer, out elementList))
            {
                managedLayers.Add(layer);
                managedLayers.Sort();
                elementList = new List<CanvasElement>();
                managedElements.Add(layer, elementList);
            }
            elementList.Add(canvasElement);
        }

        public void RemoveElement(CanvasElement canvasElement)
        {
            int layer = canvasElement.Layer;
            if (managedElements.TryGetValue(layer, out List<CanvasElement> elementList))
            {
                elementList.Remove(canvasElement);
            }
        }
    }
}
