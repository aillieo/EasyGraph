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
        private Vector2 Size { get; } = new Vector2(1280f, 720f);

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
            this.Begin();

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

            this.End();
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            return EasyGraphWindow.Instance.ViewRect.Contains(pos);
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
                            break;
                        }
                    }
                }
            }

            if(!handled)
            {
                handled = HandleGUIEvent(current);
            }
            return handled;
        }

        protected override bool OnMouseDown(Event evt)
        {
            if(evt.button == 0)
            {
                SelectUtils.currentSelected = null;
                return true;
            }
            else
            {
                if(ConnectUtils.currentBuilder.IsBuilding)
                {
                    ConnectUtils.currentBuilder.Abandon();
                    return true;
                }
            }
            return false;
        }

        protected override bool OnMouseDrag(Event evt)
        {
            if(evt.button == 2)
            {
                Offset += evt.delta / Scale;
                CheckBounds();
                return true;
            }
            return false;
        }

        protected override bool OnScroll(Event evt)
        {
            float newScale = Scale - evt.delta.y * 0.005f;
            newScale = Mathf.Clamp(newScale, CanvasScaleRange.x, CanvasScaleRange.y);

            if (Mathf.Abs(newScale - Scale) > float.Epsilon)
            {
                Vector2 pos = evt.mousePosition;
                Vector2 scaledOffset = Offset * Scale;
                Vector2 scaledSize = Size * Scale;
                Vector2 start = EasyGraphWindow.Instance.ViewRect.position + scaledOffset;
                Vector2 center = start + scaledSize / 2;
                Vector2 posToCenter = pos - center;
                Vector2 newPosToCenter = posToCenter / Scale * newScale;
                Vector2 newCenter = pos - newPosToCenter;
                Vector2 newScaledSize = Size * newScale;
                Vector2 newStart = newCenter - newScaledSize / 2;
                Vector2 newScaledOffset = newStart - EasyGraphWindow.Instance.ViewRect.position;
                Offset = newScaledOffset / newScale;

                Scale = newScale;
                CheckBounds();
            }
            return true;

        }

        protected override bool OnContextClick(Event evt)
        {
            GenericMenu genericMenu = new GenericMenu();
            Vector2 posCanvas = WindowPosToCanvasPos(evt.mousePosition);

            genericMenu.AddItem(new GUIContent("Create Node"), false, () => this.AddElement(new Node(posCanvas)));
            genericMenu.AddItem(new GUIContent("Reset"), false, () => { Scale = 1; Offset = Vector2.zero; });

            genericMenu.ShowAsContext();
            return true;
        }

        private void CheckBounds()
        {
            Rect viewRect = EasyGraphWindow.Instance.ViewRect;
            float x = Mathf.Clamp(Offset.x, -Size.x, viewRect.size.x / Scale);
            float y = Mathf.Clamp(Offset.y, -Size.y, viewRect.size.y / Scale);
            Offset = new Vector2(x, y);
        }


        // 调完Begin 进入Canvas空间
        public void Begin()
        {
            // 显示实际范围
            GUI.Box(EasyGraphWindow.Instance.ViewRect, GUIContent.none, new GUIStyle("box"));

            GUI.EndGroup();

            Rect canvasRect = RectUtils.ScaleRect(EasyGraphWindow.Instance.ViewRect, 1.0f / Scale, EasyGraphWindow.Instance.ViewRect.position);
            canvasRect = RectUtils.OffsetRect(canvasRect,Vector2.up * GUIUtils.titleHeight);

            GUI.BeginGroup(canvasRect);

            Matrix4x4 translation = Matrix4x4.TRS(canvasRect.position, Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(Scale, Scale, 1.0f));
            Matrix4x4 transform = translation * scale * translation.inverse * GUI.matrix;
            GUIUtils.PushGUIMatrix(transform);
        }

        // 调完End 退出Canvas空间 返回Window空间
        public void End()
        {
            GUIUtils.PopGUIMatrix();
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, GUIUtils.titleHeight, Screen.width, Screen.height));
        }


        private void DrawGrids()
        {
            /*
            Texture2D gridTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EasyGraph/Resources/bg1.png");
            Vector2 tiles = new Vector2(
                Size.x / gridTex.width,
                Size.y / gridTex.height);

            GUI.DrawTextureWithTexCoords(
                new Rect(Offset, Size),
                gridTex,
                new Rect(Vector2.one * 0.5f, tiles)
                );
            */

            Handles.BeginGUI();
            GUIUtils.PushHandlesColor(colorDark);

            float scaledSpacing = baseGridSpacing;
            Vector2 scaledSize = Size;
            Vector2 scaledOffset = Offset;

            int xGrid = Mathf.RoundToInt(scaledSize.x / scaledSpacing);
            int yGrid = Mathf.RoundToInt(scaledSize.y / scaledSpacing);

            for (int x = 1; x < xGrid; ++x)
            {
                bool thick = (x % subGridFactor == 0);
                if (thick)
                {
                    GUIUtils.PushHandlesColor(colorLight);
                }
                Vector3 start = Vector3.right * scaledSpacing * x + (Vector3)scaledOffset;
                Vector3 end = start + Vector3.up * scaledSize.y;
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
                Vector3 start = Vector3.up * y * scaledSpacing + (Vector3)scaledOffset;
                Vector3 end = start + Vector3.right * scaledSize.x;
                Handles.DrawLine(start, end);
                if (thick)
                {
                    GUIUtils.PopHandlesColor();
                }
            }

            GUIUtils.PopHandlesColor();
            Handles.EndGUI();
        }

        public Vector2 WindowPosToCanvasPos(Vector2 windowPos)
        {
            return windowPos / this.Scale - this.Offset - EasyGraphWindow.Instance.ViewRect.position / Scale;
        }

        public Vector2 CanvasPosToWindowPos(Vector2 canvasPos)
        {
            return canvasPos * Scale + Offset * Scale + EasyGraphWindow.Instance.ViewRect.position;
        }

        public Rect CanvasRectToWindowRect(Rect canvasRect)
        {
            Rect rect = RectUtils.ScaleRect(canvasRect, Scale);
            rect.position = CanvasPosToWindowPos(rect.position);
            return rect;
        }

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
            CanvasElement.Add(canvasElement);
            elementList.Add(canvasElement);
        }

        public void RemoveElement(CanvasElement canvasElement)
        {
            int layer = canvasElement.Layer;
            if (managedElements.TryGetValue(layer, out List<CanvasElement> elementList))
            {
                elementList.Remove(canvasElement);
                CanvasElement.Remove(canvasElement);
            }
        }

    }
}
