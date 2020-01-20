using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasElement : CanvasObject
    {
        public abstract int Layer { get; }

        public Canvas canvas { get; protected set; }


        public static void Draw(CanvasElement canvasElement)
        {
            if (canvasElement != null)
            {
                canvasElement.OnDraw();
            }
        }

        protected abstract void OnDraw();

        public static void Add(Canvas canvas, CanvasElement canvasElement)
        {
            canvasElement.canvas = canvas;
            canvasElement.OnAdd();
        }

        public static void Remove(CanvasElement canvasElement)
        {
            canvasElement.OnRemove();
            canvasElement.canvas = null;
        }

        protected abstract void OnAdd();
        protected abstract void OnRemove();
    }

}
