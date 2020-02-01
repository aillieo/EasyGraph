using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class DummyRoute<TData> : CanvasElement<TData> where TData : INodeDataWrapper
    {

        public DummyRoute(Node<TData> nodeFrom)
        {
            this.nodeFrom = nodeFrom;
        }

        private Node<TData> nodeFrom;

        public override int Layer => LayerDefine.DummyRoute;

        protected override void OnDraw()
        {
            Vector2 mousePos = Event.current.mousePosition;
            Vector2 fromCenter = nodeFrom.Rect.Offset(canvas.Offset).center;
            Vector2 horizontalDiff = Vector2.right * (fromCenter - mousePos).x;
            Vector2 horizontalDir = horizontalDiff.normalized;
            Vector2 pointFrom = fromCenter - horizontalDir * nodeFrom.Rect.width / 2;
            Handles.DrawBezier(
                pointFrom,
                mousePos,
                pointFrom - horizontalDir * 200f,
                mousePos + horizontalDir * 200f,
                Color.gray,
                null,
                4f);
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
