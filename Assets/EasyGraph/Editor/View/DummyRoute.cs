using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class DummyRoute : CanvasElement
    {

        public DummyRoute(Node nodeFrom)
        {
            this.nodeFrom = nodeFrom;
        }

        private Node nodeFrom;

        public override int Layer => LayerDefine.DummyRoute;

        protected override void OnDraw()
        {
            Vector2 mousePos = Event.current.mousePosition;
            Handles.DrawLine(
            new Rect(nodeFrom.Rect).Offset(canvas.Offset).center,
            mousePos);
            GUI.changed = true;
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            return false;
        }

        protected override void OnAdd()
        {}

        protected override void OnRemove()
        {}
    }

}
