using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyGraph
{
    public class EasyGraphWindow : EditorWindow
    {
        public static readonly float titleHeight = 23.0f;

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

        public Rect ViewRect { get; private set; } = new Rect(300f, 300f, 300f, 300f);

        private void OnGUI()
        {
            bool eventHandled = Canvas.HandleGUIEvent();

            GUI.Label(
                new Rect(ViewRect.position - 2 * Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                string.Format("Offset={0}Scale={1}", Canvas.Offset, Canvas.Scale));
            if(SelectUtils.currentSelected != null)
            {
                Node node = SelectUtils.currentSelected;
                node.data.name = GUI.TextField(
                    new Rect(ViewRect.position - Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                    node.data.name);
            }

            if (Event.current.type == EventType.Repaint)
            {
                CanvasObject.Draw(Canvas);
            }

            if(eventHandled)
            {
                Repaint();
            }
        }
    }
}
