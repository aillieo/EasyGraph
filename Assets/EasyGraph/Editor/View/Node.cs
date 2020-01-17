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


        // canvas space
        public Vector2 Position { get; protected set; }
        public virtual Vector2 Size { get; protected set; } = new Vector2(150, 80);
        public Rect Rect
        {
            get
            {
                return new Rect(Position, Size);
                //return RectUtils.OffsetRect(new Rect(Position, Size), EasyGraphWindow.CurrentCanvas.Offset);
            } 
        }

        protected virtual GUIStyle Style => new GUIStyle("window");

        protected override void OnDraw()
        {
            Rect position = RectUtils.OffsetRect(new Rect(Position, Size), EasyGraphWindow.CurrentCanvas.Offset);
            if (this == SelectUtils.currentSelected)
            {
                GUIUtils.PushGUIColor(Color.blue);
                GUI.Box(RectUtils.ScaleRect(position, 1.1f, position.center),"", new GUIStyle("box"));
                GUIUtils.PopGUIColor();
            }
            GUI.Box(position, $"node{this.Position}", Style);
            GUI.Label(
                new Rect(position.position + EditorGUIUtility.singleLineHeight * Vector2.up, new Vector2(position.width, EditorGUIUtility.singleLineHeight)),
                data.name);
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            Rect r = EasyGraphWindow.CurrentCanvas.CanvasRectToWindowRect(Rect);
            return r.Contains(pos);
        }

        protected override bool OnMouseDrag(Event evt)
        {
            if (evt.button == 0)
            {
                Position += evt.delta / EasyGraphWindow.CurrentCanvas.Scale;
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Event evt)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Make link"), false, () => ConnectUtils.currentBuilder.StartWith(this));
            genericMenu.AddItem(new GUIContent("Detach Node"), false, () => ConnectUtils.RemoveAllConnections(this));
            genericMenu.AddItem(new GUIContent("Remove Node"), false, () => EasyGraphWindow.CurrentCanvas.RemoveElement(this));
            genericMenu.DropDown(new Rect(evt.mousePosition,Vector2.zero));
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

    }

}
