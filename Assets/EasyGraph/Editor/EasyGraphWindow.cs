using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyGraph
{
    public class EasyGraphWindow : EditorWindow
    {
        //private readonly Canvas rootCanvas = new Canvas(new Rect(0, 0, 500f, 300f));
        private readonly Canvas rootCanvas = new Canvas(new Rect(30, 30, 500, 300f));

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
            if (CurrentCanvas.HandleGUIEvent())
            {
                GUI.changed = true;
            }

            if (Event.current.type == EventType.Repaint)
            {
                CanvasObject.Draw(CurrentCanvas);
            }

            GUI.Label(new Rect(CurrentCanvas.Rect),string.Format("Offset={0}Scale={1}", CurrentCanvas.Offset, CurrentCanvas.Scale));

            if (GUI.changed)
            {
                Repaint();
            }

        }

    }
}
