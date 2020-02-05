using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public static class GUIUtils
    {
        public static readonly float titleHeight = 23.0f;

        #region color management
        private static Stack<Color> guiColorStack = new Stack<Color>();
        public static void PushGUIColor(Color color)
        {
            guiColorStack.Push(GUI.color);
            GUI.color = color;
        }
        public static void PopGUIColor()
        {
            GUI.color = guiColorStack.Pop();
        }
        private static Stack<Color> handlesColorStack = new Stack<Color>();
        public static void PushHandlesColor(Color color)
        {
            handlesColorStack.Push(Handles.color);
            Handles.color = color;
        }
        public static void PopHandlesColor()
        {
            Handles.color = handlesColorStack.Pop();
        }
        #endregion

        #region matrix
        private static Stack<Matrix4x4> guiMatrixStack = new Stack<Matrix4x4>();
        public static void PushGUIMatrix(Matrix4x4 matrix)
        {
            guiMatrixStack.Push(GUI.matrix);
            GUI.matrix = matrix;
        }
        public static void PopGUIMatrix()
        {
            GUI.matrix = guiMatrixStack.Pop();
        }
        #endregion

        #region bezier

        private static float bezierStep = 0.0005f;
        private static readonly Vector3[] vecBuffer = new Vector3[2];

        private static void DrawAAPolyLine(float width, Vector2 p0, Vector2 p1)
        {
            vecBuffer[0].x = p0.x;
            vecBuffer[0].y = p0.y;
            vecBuffer[1].x = p1.x;
            vecBuffer[1].y = p1.y;
            Handles.DrawAAPolyLine(width, vecBuffer);
        }

        public static Vector2 GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float tt = t * t;
            float ttt = tt * t;

            float u = 1 - t;
            float uu = u * u;
            float uuu = uu * u;

            float x = (uuu * p0.x) + (3 * uu * t * p1.x) + (3 * u * tt * p2.x) + (ttt * p3.x);
            float y = (uuu * p0.y) + (3 * uu * t * p1.y) + (3 * u * tt * p2.y) + (ttt * p3.y);

            return new Vector2(x,y);
        }

        public static int GetBezierPoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, IList<Vector2> points)
        {
            points.Clear();
            int segCount = Mathf.CeilToInt((p0 - p3).sqrMagnitude * bezierStep) + 3;
            points.Add(p0);
            for (int i = 1; i < segCount; ++i)
            {
                points.Add(GetBezierPoint(p0,p1,p2,p3,((float)i)/ segCount));
            }
            points.Add(p3);
            return points.Count;
        }

        public static void DrawBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float width)
        {
            Handles.DrawBezier(p0, p3, p1, p2, color, null, width);
        }

        public static int DrawBezierPolyLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float width)
        {
            int segCount = Mathf.CeilToInt((p0 - p3).sqrMagnitude * bezierStep) + 3;
            Vector2 lastPoint = GetBezierPoint(p0, p1, p2, p3, 0);
            for (int i = 0; i < segCount; ++i)
            {
                Vector2 newPoint = GetBezierPoint(p0,p1,p2,p3,((float)i + 1)/ segCount);
                DrawAAPolyLine(width,lastPoint,newPoint);
                lastPoint = newPoint;
            }
            return segCount;
        }
        #endregion

    }
}
