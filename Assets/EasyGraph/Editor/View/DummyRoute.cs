using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class DummyRoute<TNodeData,TRouteData> : CanvasElement<TNodeData,TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {

        public DummyRoute(Node<TNodeData,TRouteData> nodeFrom)
        {
            this.nodeFrom = nodeFrom;
        }

        private Node<TNodeData,TRouteData> nodeFrom;

        public override int Layer => LayerDefine.DummyRoute;

        protected override void OnDraw()
        {
            Vector2 point3 = Event.current.mousePosition;
            Vector2 point0 = nodeFrom.Rect.center + canvas.Offset;
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
