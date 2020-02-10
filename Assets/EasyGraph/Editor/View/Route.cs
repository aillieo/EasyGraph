using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Route<TNodeData, TRouteData> : CanvasElement<TNodeData, TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {

        public readonly Node<TNodeData, TRouteData> nodeFrom;
        public readonly Node<TNodeData, TRouteData> nodeTo;

        public readonly TRouteData data;

        public Route(Node<TNodeData, TRouteData> nodeFrom, Node<TNodeData, TRouteData> nodeTo, TRouteData data)
        {
            this.data = data;
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
        }

        public Route(Node<TNodeData, TRouteData> nodeFrom, Node<TNodeData, TRouteData> nodeTo)
            :this(nodeFrom,nodeTo,new TRouteData())
        {}

        public override int Layer => LayerDefine.Route;

        protected override void OnDraw()
        {
            Vector2 horizontalDiff = Vector2.right * (nodeFrom.Position - nodeTo.Position).x;
            Vector2 horizontalDir = horizontalDiff.normalized;
            Vector2 point0 = nodeFrom.Rect.center + canvas.Offset;
            Vector2 point1 = point0 - horizontalDir * nodeFrom.Rect.width;
            Vector2 point3 = nodeTo.Rect.center + canvas.Offset;
            Vector2 point2 = point3 + horizontalDir * nodeFrom.Rect.width;

            GUIUtils.DrawBezier(
                point0,
                point1,
                point2,
                point3,
                this.data.GetColor(),
                4f);

            //Handles.DrawAAPolyLine(6,bezierPoints.Select((v2)=>(Vector3)v2).ToArray());

            }

        private static readonly List<Vector2> bezierPoints = new List<Vector2>();
        protected override bool RectContainsPoint(Vector2 pos)
        {
            Vector2 horizontalDiff = Vector2.right * (nodeFrom.Position - nodeTo.Position).x;
            Vector2 horizontalDir = horizontalDiff.normalized;
            Vector2 point0 = nodeFrom.Rect.center; // + canvas.Offset;
            Vector2 point1 = point0 - horizontalDir * nodeFrom.Rect.width;
            Vector2 point3 = nodeTo.Rect.center; // + canvas.Offset;
            Vector2 point2 = point3 + horizontalDir * nodeFrom.Rect.width;

            Vector2 canvasPos = canvas.WindowPosToCanvasPos(pos);
            if (canvasPos.x < Mathf.Min(point0.x,point3.x)
                ||canvasPos.y < Mathf.Min(point0.y,point3.y)
                ||canvasPos.x > Mathf.Max(point0.x,point3.x)
                ||canvasPos.y > Mathf.Max(point0.y,point3.y))
            {
                return false;
            }

            GUIUtils.GetBezierPoints(
                point0,
                point1,
                point2,
                point3,
                bezierPoints);
            foreach (var point in bezierPoints )
            {
                if ((point - canvasPos).sqrMagnitude < 100)
                {
                    return true;
                }
            }

            return false;
        }

        protected override bool OnMouseDown(Event evt)
        {
            if(evt.button == 0 && this.data.Selectable)
            {
                if (!canvas.operation.selection.HasSelected(this))
                {
                    canvas.operation.selection.Clear();
                    canvas.operation.selection.Select(this);
                }
                return true;
            }
            return false;
        }

        protected override bool OnContextClick(Event evt)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove Route"), false, () => canvas.RemoveElement(this));
            genericMenu.ShowAsContext();
            return true;
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

    public class Route<TNodeData>
        : Route<TNodeData, DefaultRouteDataWrapper>
        where TNodeData : INodeDataWrapper
    {
        public Route(Node<TNodeData> nodeFrom, Node<TNodeData> nodeTo):base(nodeFrom, nodeTo)
        {}
    }

}

