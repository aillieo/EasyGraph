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

        protected override bool OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            Rect r = EasyGraphWindow.CurrentCanvas.CanvasRectToWindowRect(Rect);

            if (r.Contains(pos))
            {
                Position += delta / EasyGraphWindow.CurrentCanvas.Scale;
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Vector2 pos)
        {
            Rect r = EasyGraphWindow.CurrentCanvas.CanvasRectToWindowRect(Rect);

            if (r.Contains(pos))
            { 
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Make link"), false, () => ConnectUtils.currentBuilder.StartWith(this));
                genericMenu.AddItem(new GUIContent("Remove Node"), false, () => EasyGraphWindow.CurrentCanvas.RemoveElement(this));
                genericMenu.DropDown(new Rect(pos,Vector2.zero));
                return true;
            }
            return false;
        }

        protected override bool OnMouseDown(Vector2 pos)
        {
            Rect r = EasyGraphWindow.CurrentCanvas.CanvasRectToWindowRect(Rect);

            if (!r.Contains(pos))
            {
                return false;
            }
            if(ConnectUtils.currentBuilder.IsBuilding)
            {
                ConnectUtils.currentBuilder.FinishWith(this);
                return true;
            }
            if(SelectUtils.currentSelected != this)
            {
                SelectUtils.currentSelected = this;
                return true;
            }

            return false;
        }

    }

}
