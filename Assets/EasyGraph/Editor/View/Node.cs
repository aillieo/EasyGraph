using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Node<TNodeData, TRouteData> : CanvasElement<TNodeData, TRouteData>
        where TNodeData: INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        public Node(TNodeData data, Vector2 canvasPos)
        {
            this.data = data;
            this.Position = canvasPos;
        }

        public readonly TNodeData data;

        public override int Layer => LayerDefine.Node;

        internal readonly HashSet<Route<TNodeData, TRouteData>> associatedRoutes = new HashSet<Route<TNodeData, TRouteData>>();

        // canvas space
        public Vector2 Position { get; protected set; }

        public Rect Rect
        {
            get
            {
                return new Rect(Position, data.Size);
            }
        }

        protected override void OnDraw()
        {
            Rect position = new Rect(Position, data.Size).Offset(canvas.Offset);
            if (canvas.operation.selection.HasSelected(this))
            {
                GUIUtils.PushGUIColor(Color.blue);
                GUI.Box(new Rect(position).Scale(1.1f, position.center), GUIContent.none, new GUIStyle("box"));
                GUIUtils.PopGUIColor();
            }
            // GUI.Box(position, $"node{this.Position}", new GUIStyle("window"));
            GUILayout.BeginArea(position);
            data.OnGUI(new Rect(Vector2.zero,position.size));
            GUILayout.EndArea();
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            Rect r = canvas.CanvasRectToWindowRect(Rect);
            return r.Contains(pos);
        }

        protected override bool OnMouseDrag(Event evt)
        {
            if (evt.button == 0)
            {
                Position += evt.delta / canvas.Scale;
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Event evt)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Make Route"), false, () => canvas.operation.connection.StartWith(this));
            genericMenu.AddItem(new GUIContent("Remove All Routes"), false, () => RemoveAllRoutes());
            genericMenu.AddItem(new GUIContent("Remove Node"), false, () => canvas.RemoveElement(this));
            genericMenu.ShowAsContext();
            return true;
        }

        protected override bool OnMouseDown(Event evt)
        {
            if(evt.button == 0)
            {
                if (canvas.operation.connection.IsBuilding)
                {
                    canvas.operation.connection.FinishWith(this);
                }
                if (!canvas.operation.selection.HasSelected(this))
                {
                    canvas.operation.selection.Clear();
                    canvas.operation.selection.Select(this);
                }
                return true;
            }
            return false;
        }

        protected override void OnAdd()
        { }

        protected override void OnRemove()
        {
            RemoveAllRoutes();
        }

        public void RemoveAllRoutes()
        {
            Route<TNodeData, TRouteData>[] routes = new Route<TNodeData, TRouteData>[associatedRoutes.Count];
            associatedRoutes.CopyTo(routes);
            foreach (var r in routes)
            {
                canvas.RemoveElement(r);
            }
        }
    }

}
