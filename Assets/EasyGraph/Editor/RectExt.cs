using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils
{
    public static class RectExt
    {
        public static Rect Scale(this Rect rect, float scale)
        {
            return rect.Scale(scale, rect.min);
        }

        public static Rect Scale(this Rect rect, float scale, Vector2 pivotPoint)
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

        public static Rect Offset(this Rect rect, Vector2 offset)
        {
            rect.x += offset.x;
            rect.y += offset.y;
            return rect;
        }

        public static Rect OffsetX(this Rect rect, float offset)
        {
            rect.x += offset;
            return rect;
        }

        public static Rect OffsetY(this Rect rect, float offset)
        {
            rect.y += offset;
            return rect;
        }

        public static Rect SetHeight(this Rect rect, float height)
        {
            rect.height = height;
            return rect;
        }

        public static Rect SetWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        public static void Encapsulate(this ref Rect rect, Vector2 point)
        {
            Vector2 min = Vector2.Min(rect.position, point);
            Vector2 max = Vector2.Max(rect.max, point);
            rect.position = min;
            rect.size = max - min;
        }

        public static void Encapsulate(this ref Rect rect, Rect bounds)
        {
            Vector2 min = Vector2.Min(rect.position, bounds.position);
            Vector2 max = Vector2.Max(rect.max, bounds.max);
            rect.position = min;
            rect.size = max - min;
        }

    }
}
