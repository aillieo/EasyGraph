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
            Vector2 point3 = Event.current.mousePosition;
            Vector2 point0 = nodeFrom.Rect.Offset(canvas.Offset).center;
            Vector2 horizontalDiff = Vector2.right * (point0 - point3).x;
            Vector2 horizontalDir = horizontalDiff.normalized;
            Vector2 point1 = point0 - horizontalDir * nodeFrom.Rect.width / 2;
            Vector2 point2 = point3;

            GUIUtils.DrawBezier(
                point0,
                point1,
                point2,
                point3,
                Color.gray,
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
