using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasElement<TNodeData, TRouteData>
        : CanvasObject
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        public abstract int Layer { get; }

        public Canvas<TNodeData, TRouteData> canvas { get; protected set; }


        public static void Draw(CanvasElement<TNodeData, TRouteData> canvasElement)
        {
            if (canvasElement != null)
            {
                canvasElement.OnDraw();
            }
        }

        protected abstract void OnDraw();

        public static void Add(Canvas<TNodeData, TRouteData> canvas, CanvasElement<TNodeData, TRouteData> canvasElement)
        {
            canvasElement.canvas = canvas;
            canvasElement.OnAdd();
        }

        public static void Remove(CanvasElement<TNodeData, TRouteData> canvasElement)
        {
            canvasElement.OnRemove();
            canvasElement.canvas = null;
        }

        protected abstract void OnAdd();
        protected abstract void OnRemove();
    }
}
