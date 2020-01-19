using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public abstract class CanvasElement : CanvasObject
    {
        public abstract int Layer { get; }

        public static void Add(CanvasElement canvasElement)
        {
            canvasElement.OnAdd();
        }

        public static void Remove(CanvasElement canvasElement)
        {
            canvasElement.OnRemove();
        }

        protected abstract void OnAdd();
        protected abstract void OnRemove();
    }

}
