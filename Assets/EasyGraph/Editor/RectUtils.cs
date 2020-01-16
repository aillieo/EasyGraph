using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public static class RectUtils
    {
        public static Rect ScaleRect(Rect rect, float scale)
        {
            return ScaleRect(rect, scale, rect.min);
        }
        public static Rect ScaleRect(Rect rect, float scale, Vector2 pivotPoint)
        {
            rect.x -= pivotPoint.x;
            rect.y -= pivotPoint.y;
            rect.xMin *= scale;
            rect.xMax *= scale;
            rect.yMin *= scale;
            rect.yMax *= scale;
            rect.x += pivotPoint.x;
            rect.y += pivotPoint.y;
            return rect;
        }

        public static Rect OffsetRect(Rect rect, Vector2 offset)
        {
            rect.x += offset.x;
            rect.y += offset.y;
            return rect;
        }

        public static Vector2 GetLeftTop(Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Vector2 GetRightBottom(Rect rect)
        {
            return new Vector2(rect.xMax, rect.yMax);
        }
    }
}
