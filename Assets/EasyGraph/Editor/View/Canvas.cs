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
        public Canvas(Rect rect)
        {
            this.Rect = rect;
        }

        public float Scale { get; private set; } = 1.0f;
        public Vector2 Offset { get; private set; } = Vector2.zero;
        public Rect Rect { get; private set; }


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

        protected override bool OnMouseDown(Vector2 pos)
        {
            if(Rect.Contains(pos))
            {
                SelectUtils.currentSelected = null;
                return true;
            }
            return false;
        }

        protected override bool OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            if(Rect.Contains(pos))
            {
                Offset += delta / Scale;
                CheckBounds();
                return true;
            }
            return false;
        }

        protected override bool OnScroll(Vector2 pos, float delta)
        {
            if(Rect.Contains(pos))
            {
                float newScale = Scale - delta * 0.005f;
                newScale = Mathf.Clamp(newScale, CanvasScaleRange.x, CanvasScaleRange.y);

                if (Mathf.Abs(newScale - Scale) > float.Epsilon)
                {
                    Vector2 scaledOffset = Offset * Scale;
                    Vector2 scaledSize = Rect.size * Scale;
                    Vector2 start = Rect.position + scaledOffset;
                    Vector2 center = start + scaledSize / 2;
                    Vector2 posToCenter = pos - center;
                    Vector2 newPosToCenter = posToCenter / Scale * newScale;
                    Vector2 newCenter = pos - newPosToCenter;
                    Vector2 newScaledSize = Rect.size * newScale;
                    Vector2 newStart = newCenter - newScaledSize / 2;
                    Vector2 newScaledOffset = newStart - Rect.position;
                    Offset = newScaledOffset / newScale;

                    Scale = newScale;
                    CheckBounds();
                }
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Vector2 pos)
        {
            if(Rect.Contains(pos))
            {
                GenericMenu genericMenu = new GenericMenu();
                Vector2 posCanvas = WindowPosToCanvasPos(pos) - Offset * 2;

                genericMenu.AddItem(new GUIContent("Create Node"), false, () => this.AddElement(new Node(posCanvas)));
                genericMenu.AddItem(new GUIContent("Reset"), false, () => { Scale = 1; Offset = Vector2.zero; });

                genericMenu.ShowAsContext();
                return true;
            }
            return false;
        }

        private void CheckBounds()
        {
            float x = Mathf.Clamp(Offset.x, Rect.size.x / Scale - Rect.size.x / Scale - Rect.size.x, Rect.size.x / Scale);
            float y = Mathf.Clamp(Offset.y, Rect.size.y / Scale - Rect.size.y / Scale - Rect.size.y, Rect.size.y / Scale);
            Offset = new Vector2(x, y);
        }


        // 调完Begin 进入Canvas空间
        public void Begin()
        {
            GUI.Box(this.Rect, GUIContent.none, new GUIStyle("box"));

            GUI.EndGroup();

            Rect canvasRect = RectUtils.ScaleRect(this.Rect, 1.0f / Scale, RectUtils.GetLeftTop(this.Rect));
            canvasRect = RectUtils.OffsetRect(canvasRect,Vector2.up * EasyGraphWindow.titleHeight);

            GUI.BeginGroup(canvasRect);

            Matrix4x4 translation = Matrix4x4.TRS(RectUtils.GetLeftTop(canvasRect), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(Scale, Scale, 1.0f));
            Matrix4x4 transform = translation * scale * translation.inverse * GUI.matrix;
            GUIUtils.PushGUIMatrix(transform);
        }

        // 调完End 退出Canvas空间 返回Window空间
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

            float scaledSpacing = baseGridSpacing;// * Scale;
            Vector2 scaledSize = Rect.size;// * Scale;
            Vector2 scaledOffset = Offset - EasyGraphWindow.CurrentCanvas.Rect.position;// * Scale;

            int xGrid = Mathf.RoundToInt(scaledSize.x / scaledSpacing);
            int yGrid = Mathf.RoundToInt(scaledSize.y / scaledSpacing);

            for (int x = 1; x < xGrid; ++x)
            {
                bool thick = (x % subGridFactor == 0);
                if (thick)
                {
                    GUIUtils.PushHandlesColor(colorLight);
                }
                Vector3 start = (Vector3)Rect.position + Vector3.right * scaledSpacing * x + (Vector3)scaledOffset;
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
                Vector3 start = (Vector3)Rect.position + Vector3.up * y * scaledSpacing + (Vector3)scaledOffset;
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
            return (windowPos - RectUtils.GetLeftTop(this.Rect)) / this.Scale + this.Offset;
        }

        public Vector2 CanvasPosToWindowPos(Vector2 canvasPos)
        {
            return canvasPos * Scale + Offset * Scale;
            //return (canvasPos - this.Offset) * this.Scale + RectUtils.GetLeftTop(this.Rect);
        }

        public Rect CanvasRectToWindowRect(Rect canvasRect)
        {
            Rect rect = RectUtils.ScaleRect(canvasRect, Scale);
            rect.position = CanvasPosToWindowPos(rect.position);
            rect.position += EasyGraphWindow.CurrentCanvas.Rect.position;
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
