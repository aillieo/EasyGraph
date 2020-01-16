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

        public override int Layer => LayerDefine.Node;


        // canvas space
        public Vector2 Position { get; protected set; }
        public virtual Vector2 Size { get; protected set; } = new Vector2(150, 80);
        public Rect Rect
        {
            get
            {
                //return new Rect(Position, Size);
                return RectUtils.OffsetRect(new Rect(Position, Size), EasyGraphWindow.CurrentCanvas.Offset);
            } 
        }

        protected virtual GUIStyle Style => new GUIStyle("window");

        protected override void OnDraw()
        {
            GUI.Box(Rect, $"node{this.Position}", Style);
            //GUI.Box(RectUtils.OffsetRect(new Rect(Position, Size), EasyGraphWindow.CurrentCanvas.Offset), "node" + this.Position, Style);
        }

        protected override bool OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            //pos = EasyGraphWindow.CurrentCanvas.WindowPosToCanvasPos(pos) - EasyGraphWindow.CurrentCanvas. Offset * 2;

            if (Rect.Contains(pos))
            {
                Position += delta;
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Vector2 pos)
        {
            pos = EasyGraphWindow.CurrentCanvas.WindowPosToCanvasPos(pos) - EasyGraphWindow.CurrentCanvas. Offset * 0;

            // 转移到Canvas中判断
            if (Rect.Contains(pos))
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
            if (!Rect.Contains(pos))
            {
                return false;
            }
            if(ConnectUtils.currentBuilder.IsBuilding)
            {
                ConnectUtils.currentBuilder.FinishWith(this);
                return true;
            }

            return false;
        }

    }

}
