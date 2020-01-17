using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyGraph
{
    public class EasyGraphWindow : EditorWindow
    {
        private readonly Canvas rootCanvas = new Canvas();

        public static readonly float titleHeight = 23.0f;

        private Canvas topCanvas;
        public static Canvas CurrentCanvas
        {
            get
            {
                return Instance.topCanvas ?? Instance.rootCanvas;
            }
            set
            {
                Instance.topCanvas = value;
                Instance.OnCanvasChanged?.Invoke(value);
            }
        }


        public event Action<Canvas> OnCanvasChanged;

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

        private void OnGUI()
        {
            bool eventHandled = CurrentCanvas.HandleGUIEvent();

            GUI.Label(
                new Rect(CurrentCanvas.Rect.position - 2 * Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                string.Format("Offset={0}Scale={1}", CurrentCanvas.Offset, CurrentCanvas.Scale));
            if(SelectUtils.currentSelected != null)
            {
                Node node = SelectUtils.currentSelected;
                node.data.name = GUI.TextField(
                    new Rect(CurrentCanvas.Rect.position - Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                    node.data.name);
            }

            if (Event.current.type == EventType.Repaint)
            {
                CanvasObject.Draw(CurrentCanvas);
            }

            if(eventHandled)
            {
                Repaint();
            }
        }
    }
}
