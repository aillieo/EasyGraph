using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasElement<TData> : CanvasObject<TData> where TData : INodeDataWrapper
    {
        public abstract int Layer { get; }

        public Canvas<TData> canvas { get; protected set; }


        public static void Draw(CanvasElement<TData> canvasElement)
        {
            if (canvasElement != null)
            {
                canvasElement.OnDraw();
            }
        }

        protected abstract void OnDraw();

        public static void Add(Canvas<TData> canvas, CanvasElement<TData> canvasElement)
        {
            canvasElement.canvas = canvas;
            canvasElement.OnAdd();
        }

        public static void Remove(CanvasElement<TData> canvasElement)
        {
            canvasElement.OnRemove();
            canvasElement.canvas = null;
        }

        protected abstract void OnAdd();
        protected abstract void OnRemove();
    }

}
