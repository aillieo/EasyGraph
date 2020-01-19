using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyGraph
{
    public class EasyGraphWindow : EditorWindow
    {
        private static EasyGraphWindow instance;

        public static EasyGraphWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    OpenWindow();
                }
                return instance;
            }
        }

        [MenuItem("AillieoUtils/Easy Graph Window")]
        private static void OpenWindow()
        {
            instance = GetWindow<EasyGraphWindow>("Easy Graph Window");
        }

        public Canvas Canvas { get; private set; } = new Canvas();

        //public Rect ViewRect { get; private set; } = new Rect(300f, 300f, 300f, 300f);
        public Rect ViewRect { get { return new Rect(Vector2.up * 3 * EditorGUIUtility.singleLineHeight, position.size - Vector2.up * EditorGUIUtility.singleLineHeight * 6); } }

        private void OnGUI()
        {
            GUI.Label(
                new Rect(Vector2.zero, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                string.Format("Offset={0}Scale={1}", Canvas.Offset, Canvas.Scale));
            if(SelectUtils.currentSelected != null)
            {
                Node node = SelectUtils.currentSelected;
                node.data.name = GUI.TextField(
                    new Rect(Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                    node.data.name);
            }

            bool eventHandled = Canvas.HandleGUIEvent();

            CanvasObject.Draw(Canvas);

            if(GUI.Button(
                new Rect(ViewRect.position + Vector2.up * (ViewRect.size.y + EditorGUIUtility.singleLineHeight), new Vector2(200, EditorGUIUtility.singleLineHeight)),
                "Save"))
            {
                Debug.Log("save");
            }

            if(eventHandled)
            {
                Repaint();
            }
        }
    }
}
