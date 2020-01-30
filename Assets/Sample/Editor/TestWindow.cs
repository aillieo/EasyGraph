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

    private Graph<NodeData,GraphAsset> graph = new Graph<NodeData, GraphAsset>(new Vector2(1280f, 640f));

    readonly string filePath = "Assets/Sample/TestGraphAsset.asset";

    private void OnGUI()
    {
        float singleLineHeight = EditorGUIUtility.singleLineHeight;

        Rect pos = position;
        pos.position = Vector2.zero;

        Rect viewRect = pos.OffsetY(3 * singleLineHeight).SetHeight(pos.size.y - singleLineHeight * 6 - GUIUtils.titleHeight);

        graph.OnGUI(viewRect);

        graph.OnGUINodeDetail(viewRect.OffsetY(-2 * singleLineHeight).SetHeight(singleLineHeight).SetWidth(200));

        if (GUI.Button(viewRect.OffsetY(viewRect.height).SetHeight(singleLineHeight).SetWidth(200),"Save"))
        {
            GraphAsset asset = CreateInstance<GraphAsset>();
            if(graph.Save(asset))
            {
                AssetDatabase.CreateAsset(asset, filePath);
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
            }
        }

        if (GUI.Button(viewRect.OffsetY(viewRect.height + 2 * singleLineHeight).SetHeight(singleLineHeight).SetWidth(200),"Load"))
        {
            GraphAsset data = AssetDatabase.LoadAssetAtPath<GraphAsset>(filePath);
            if(data != null)
            {
                Graph<NodeData, GraphAsset> newGraph = null; 
                if(Graph<NodeData, GraphAsset>.Load(data,out newGraph))
                {
                    graph = newGraph;
                }
            }
        }
    }
}
