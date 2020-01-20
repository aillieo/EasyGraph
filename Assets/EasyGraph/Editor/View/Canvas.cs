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
        public Canvas(Vector2 size)
        {
            this.Size = size;
        }

        public float Scale { get; private set; } = 1.0f;
        public Vector2 Offset { get; private set; } = Vector2.zero;

        private readonly Vector2 Size;

        public static readonly Vector2 CanvasScaleRange = new Vector2(0.2f, 5f);

        // SortedDictionary 不能逆序遍历
        private readonly Dictionary<int, List<CanvasElement>> managedElements = new Dictionary<int, List<CanvasElement>>();
        private readonly List<int> managedLayers = new List<int>();

        private Rect cachedViewRect;

        public void OnGUI(Rect viewRect)
        {
            cachedViewRect = viewRect;

            bool eventHandled = HandleGUIEvent();

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
                            CanvasElement.Draw(ele);
                        }
                    }
                }
            }

            this.End();
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            return cachedViewRect.Contains(pos);
        }

        public bool HandleGUIEvent()
        {
            Event current = Event.current;

            bool handled = false;

            if (current.isMouse && !cachedViewRect.Contains(current.mousePosition))
            {
                return false;
            }

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
                Vector2 position = cachedViewRect.position;
                Vector2 pos = evt.mousePosition;
                Vector2 scaledOffset = Offset * Scale;
                Vector2 scaledSize = Size * Scale;
                Vector2 start = position + scaledOffset;
                Vector2 center = start + scaledSize / 2;
                Vector2 posToCenter = pos - center;
                Vector2 newPosToCenter = posToCenter / Scale * newScale;
                Vector2 newCenter = pos - newPosToCenter;
                Vector2 newScaledSize = Size * newScale;
                Vector2 newStart = newCenter - newScaledSize / 2;
                Vector2 newScaledOffset = newStart - position;
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
            genericMenu.AddItem(new GUIContent("Fit All"), false, FitAllNodes);

            genericMenu.ShowAsContext();
            return true;
        }

        private void CheckBounds()
        {
            float x = Mathf.Clamp(Offset.x, -Size.x, cachedViewRect.size.x / Scale);
            float y = Mathf.Clamp(Offset.y, -Size.y, cachedViewRect.size.y / Scale);
            Offset = new Vector2(x, y);
        }


        // 调完Begin 进入Canvas空间
        private void Begin()
        {
            // 显示实际范围
            GUI.Box(cachedViewRect, GUIContent.none, new GUIStyle("box"));

            GUI.EndGroup();

            Rect canvasRect = new Rect(cachedViewRect).Scale(1.0f / Scale);
            canvasRect = canvasRect.Offset(Vector2.up * GUIUtils.titleHeight);

            GUI.BeginGroup(canvasRect);

            Matrix4x4 translation = Matrix4x4.TRS(canvasRect.position, Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(Scale, Scale, 1.0f));
            Matrix4x4 transform = translation * scale * translation.inverse * GUI.matrix;
            GUIUtils.PushGUIMatrix(transform);
        }

        // 调完End 退出Canvas空间 返回Window空间
        private void End()
        {
            GUIUtils.PopGUIMatrix();
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, GUIUtils.titleHeight, Screen.width, Screen.height));
        }


        private void DrawGrids()
        {
            Texture2D gridTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/EasyGraph/Resources/bg1.png");
            Vector2 tiles = new Vector2(
                Size.x / gridTex.width,
                Size.y / gridTex.height);

            GUI.DrawTextureWithTexCoords(
                new Rect(Offset, Size),
                gridTex,
                new Rect(Vector2.one * 0.5f, tiles)
                );
        }

        public Vector2 WindowPosToCanvasPos(Vector2 windowPos)
        {
            return windowPos / this.Scale - this.Offset - cachedViewRect.position / Scale;
        }

        public Vector2 CanvasPosToWindowPos(Vector2 canvasPos)
        {
            return canvasPos * Scale + Offset * Scale + cachedViewRect.position;
        }

        public Rect CanvasRectToWindowRect(Rect canvasRect)
        {
            Rect rect = canvasRect.Scale(Scale);
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
            CanvasElement.Add(this, canvasElement);
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


        private void FitAllNodes()
        {
            Rect rect = new Rect(Size/2 , Vector2.zero);
            List<CanvasElement> elementList;
            if (managedElements.TryGetValue(LayerDefine.Node,out elementList))
            {
                if (elementList.Count > 0)
                {
                    foreach (var ele in elementList)
                    {
                        Node node = ele as Node;
                        if (node != null)
                        {
                            rect.Encapsulate(node.Rect);
                        }
                    }
                }
            }

            Offset = (- rect.center + cachedViewRect.size / 2 / Scale);

            CheckBounds();
        }
    }
}
