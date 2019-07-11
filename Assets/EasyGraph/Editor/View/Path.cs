using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Path : CanvasElement
    {

        private readonly Node nodeFrom;
        private readonly Node nodeTo;

        public Path(Node nodeFrom, Node nodeTo)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
        }

        public override int Layer => LayerDefine.Path;

        protected override void OnDraw()
        {
            Vector2 horizonDiff = Vector2.right * (nodeFrom.Position - nodeTo.Position).x;
            Vector2 horizonDir = horizonDiff.normalized;
            Handles.DrawBezier(
                nodeFrom.Rect.center,
                nodeTo.Rect.center,
                nodeFrom.Rect.center - horizonDir * 200f,
                nodeTo.Rect.center + horizonDir * 200f,
                Color.black,
                null,
                2f);
        }

    }

}

