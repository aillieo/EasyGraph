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
            Vector2 pointFrom = RectUtils.OffsetRect(nodeFrom.Rect, EasyGraphWindow.CurrentCanvas.Offset).center;
            Vector2 pointTo = RectUtils.OffsetRect(nodeTo.Rect, EasyGraphWindow.CurrentCanvas.Offset).center;
            Handles.DrawBezier(
                pointFrom,
                pointTo,
                pointFrom - horizonDir * 200f,
                pointTo + horizonDir * 200f,
                Color.black,
                null,
                2f);
        }

    }

}

