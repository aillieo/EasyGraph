using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace AillieoUtils.EasyGraph
{
    public class EasyGraphWindow : EditorWindow
    {
        [MenuItem("AillieoUtils/Easy Graph Window")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<EasyGraphWindow>("Easy Graph Window");
        }

        private Canvas canvas = new Canvas(new Vector2(1280f, 640f));


        private void OnGUI()
        {
            Rect viewRect = new Rect(Vector2.up * 3 * EditorGUIUtility.singleLineHeight, position.size - Vector2.up * EditorGUIUtility.singleLineHeight * 6);

            GUI.Label(
                new Rect(Vector2.zero, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                string.Format("Offset={0}Scale={1}", canvas.Offset, canvas.Scale));
            if(SelectUtils.currentSelected != null)
            {
                Node node = SelectUtils.currentSelected;
                node.data.name = GUI.TextField(
                    new Rect(Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(200, EditorGUIUtility.singleLineHeight)),
                    node.data.name);
            }

            canvas.OnGUI(viewRect);

            if (GUI.Button(
                new Rect(viewRect.position + Vector2.up * (viewRect.size.y + EditorGUIUtility.singleLineHeight), new Vector2(200, EditorGUIUtility.singleLineHeight)),
                "Save"))
            {
                Debug.Log("save");
            }
        }
    }
}
