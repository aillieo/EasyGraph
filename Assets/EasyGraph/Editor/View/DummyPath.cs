using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class DummyPath : CanvasElement
    {

        public DummyPath(Node nodeFrom)
        {
            this.nodeFrom = nodeFrom;
        }

        private Node nodeFrom;

        public override int Layer => LayerDefine.DummyPath;

        protected override void OnDraw()
        {
            Vector2 mousePos = Event.current.mousePosition;
            Handles.DrawLine(
            nodeFrom.Rect.center,
            mousePos);
            GUI.changed = true;
        }

    }

}
