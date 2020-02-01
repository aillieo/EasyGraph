using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Route<TData> : CanvasElement<TData> where TData : INodeDataWrapper
    {

        public readonly Node<TData> nodeFrom;
        public readonly Node<TData> nodeTo;

        public Route(Node<TData> nodeFrom, Node<TData> nodeTo)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
        }

        public override int Layer => LayerDefine.Route;

        protected override void OnDraw()
        {
            Vector2 horizontalDiff = Vector2.right * (nodeFrom.Position - nodeTo.Position).x;
            Vector2 verticalDiff = Vector2.up * (nodeFrom.Position - nodeTo.Position).y;
            Vector2 horizontalDir = horizontalDiff.normalized;
            Vector2 verticalDir = verticalDiff.normalized;
            Vector2 pointFrom = new Rect(nodeFrom.Rect).Offset(canvas.Offset).center - horizontalDir * nodeFrom.Rect.width / 2 - verticalDiff * 0.01f;
            Vector2 pointTo = new Rect(nodeTo.Rect).Offset(canvas.Offset).center + horizontalDir * nodeFrom.Rect.width / 2 + verticalDiff * 0.01f;
            Handles.DrawBezier(
                pointFrom,
                pointTo,
                pointFrom - horizontalDir * 200f,
                pointTo + horizontalDir * 200f,
                Color.white,
                null,
                4f);
        }

        protected override bool RectContainsPoint(Vector2 pos)
        {
            return false;
        }

        protected override void OnAdd()
        {
            if(!this.nodeFrom.associatedRoutes.Contains(this))
            {
                this.nodeFrom.associatedRoutes.Add(this);
            }
            if (!this.nodeTo.associatedRoutes.Contains(this))
            {
                this.nodeTo.associatedRoutes.Add(this);
            }
        }

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

