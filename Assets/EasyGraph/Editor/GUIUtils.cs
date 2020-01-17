using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public static class GUIUtils
    {
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

    }
}
