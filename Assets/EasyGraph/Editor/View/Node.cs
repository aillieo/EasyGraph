using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Node : CanvasElement
    {
        public Node(Vector2 canvasPos)
        {
            this.Position = canvasPos;
        }

        public readonly DefaultNodeData data = new DefaultNodeData();

        public override int Layer => LayerDefine.Node;

        public readonly HashSet<Route> associatedRoutes = new HashSet<Route>();

        // canvas space
        public Vector2 Position { get; protected set; }
        public virtual Vector2 Size { get; protected set; } = new Vector2(150, 80);
        public Rect Rect
        {
            get
            {
                return new Rect(Position, Size);
            } 
        }

        protected virtual GUIStyle Style => new GUIStyle("window");

        protected override void OnDraw()
        {
            Rect position = new Rect(Position, Size).Offset(canvas.Offset);
            if (this == SelectUtils.currentSelected)
            {
                GUIUtils.PushGUIColor(Color.blue);
                GUI.Box(new Rect(position).Scale(1.1f, position.center), GUIContent.none, new GUIStyle("box"));
                GUIUtils.PopGUIColor();
            }
            GUI.Box(position, $"node{this.Position}", Style);
            GUI.Label(
                new Rect(position.position + EditorGUIUtility.singleLineHeight * Vector2.up, new Vector2(position.width, EditorGUIUtility.singleLineHeight)),
                data.name);
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
            genericMenu.AddItem(new GUIContent("Make Route"), false, () => ConnectUtils.currentBuilder.StartWith(this));
            genericMenu.AddItem(new GUIContent("Detach Node"), false, () => ConnectUtils.RemoveAllRoutes(this));
            genericMenu.AddItem(new GUIContent("Remove Node"), false, () => canvas.RemoveElement(this));
            genericMenu.ShowAsContext();
            return true;
        }

        protected override bool OnMouseDown(Event evt)
        {
            if(evt.button == 0)
            {
                if (ConnectUtils.currentBuilder.IsBuilding)
                {
                    ConnectUtils.currentBuilder.FinishWith(this);
                }
                if (SelectUtils.currentSelected != this)
                {
                    SelectUtils.currentSelected = this;
                }
                return true;
            }
            return false;
        }

        protected override void OnAdd()
        { }

        protected override void OnRemove()
        {
            ConnectUtils.RemoveAllRoutes(this);
        }
    }

}
