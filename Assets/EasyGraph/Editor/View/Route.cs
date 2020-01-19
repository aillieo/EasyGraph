using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Route : CanvasElement
    {

        private readonly Node nodeFrom;
        private readonly Node nodeTo;

        public Route(Node nodeFrom, Node nodeTo)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
        }

        public override int Layer => LayerDefine.Route;

        protected override void OnDraw()
        {
            Vector2 horizonDiff = Vector2.right * (nodeFrom.Position - nodeTo.Position).x;
            Vector2 horizonDir = horizonDiff.normalized;
            Vector2 pointFrom = RectUtils.OffsetRect(nodeFrom.Rect, EasyGraphWindow.Instance.Canvas.Offset).center;
            Vector2 pointTo = RectUtils.OffsetRect(nodeTo.Rect, EasyGraphWindow.Instance.Canvas.Offset).center;
            Handles.DrawBezier(
                pointFrom,
                pointTo,
                pointFrom - horizonDir * 200f,
                pointTo + horizonDir * 200f,
                Color.black,
                null,
                2f);
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            return false;
        }

        protected override void OnAdd()
        { }

        protected override void OnRemove()
        {
            if(nodeFrom.associatedRoutes.Contains(this))
            {
                nodeFrom.associatedRoutes.Remove(this);
            }
            if (nodeTo.associatedRoutes.Contains(this))
            {
                nodeTo.associatedRoutes.Remove(this);
            }
        }
    }

}
