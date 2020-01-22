using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AillieoUtils.EasyGraph;
using AillieoUtils;

public class TestWindow : EditorWindow
{
    [MenuItem("Window/{ TestWindow }")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow<TestWindow>("Easy Graph Window");
    }

    private Graph<NodeData> graph = new Graph<NodeData>(new Vector2(1280f, 640f));

    string testData;

    private void OnGUI()
    {
        float singleLineHeight = EditorGUIUtility.singleLineHeight;

        Rect viewRect = position.OffsetY(3 * singleLineHeight).SetHeight(position.size.y - singleLineHeight * 6 - GUIUtils.titleHeight);

        graph.OnGUI(viewRect);

        graph.OnGUINodeDetail(viewRect.OffsetY(-2 * singleLineHeight).SetHeight(singleLineHeight).SetWidth(200));

        if (GUI.Button(viewRect.OffsetY(viewRect.height).SetHeight(singleLineHeight).SetWidth(200),"Save"))
        {
            string data = graph.Save();
            testData = data;
        }

        testData = GUI.TextArea(viewRect.OffsetY(viewRect.height + singleLineHeight).SetHeight(singleLineHeight).SetWidth(200),testData);

        if (GUI.Button(viewRect.OffsetY(viewRect.height + 2 * singleLineHeight).SetHeight(singleLineHeight).SetWidth(200),"Load"))
        {
            Graph<NodeData> newGraph = Graph<NodeData>.Load(testData);
            if(newGraph != null)
            {
                graph = newGraph;
            }
        }
    }
}
