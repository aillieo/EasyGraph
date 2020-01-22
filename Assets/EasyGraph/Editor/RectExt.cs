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
            rect.x = Mathf.Min(rect.min.x,point.x);
            rect.y = Mathf.Min(rect.min.y,point.y);
            rect.width = Mathf.Max(rect.width, point.x - rect.x);
            rect.height = Mathf.Max(rect.height, point.y - rect.y);
        }

        public static void Encapsulate(this ref Rect rect, Rect bounds)
        {
            rect.x = Mathf.Min(rect.min.x, bounds.x);
            rect.y = Mathf.Min(rect.min.y, bounds.y);
            rect.width = Mathf.Max(rect.width, bounds.max.x - rect.x);
            rect.height = Mathf.Max(rect.height, bounds.max.y - rect.y);
        }

    }
}
