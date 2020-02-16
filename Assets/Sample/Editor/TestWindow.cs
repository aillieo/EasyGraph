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

    private Graph<GraphAsset,NodeData,RouteData> graph = new Graph<GraphAsset, NodeData, RouteData>(new Vector2(1280f, 640f));

    private string filePath = "Assets/Sample/TestGraphAsset.asset";

    private void OnGUI()
    {
        float singleLineHeight = EditorGUIUtility.singleLineHeight;
        Vector2 offset = new Vector2(160, singleLineHeight * 5);
        Rect viewRect = this.position;
        viewRect.position = Vector2.zero;
        viewRect.width -= (offset.x + 2);

        // main
        graph.OnGUI(viewRect);

        // detail
        Rect detail = viewRect.SetWidth(offset.x);
        detail.position = Vector2.zero;
        detail = detail.OffsetX(position.width - offset.x);
        detail.height -= (offset.y + 2);
        graph.OnGUIDetail(detail);

        // save&load
        Rect assetIO = new Rect(detail.position, offset);
        assetIO = assetIO.OffsetY(position.height - offset.y);
        GUILayout.BeginArea(assetIO);

        float move = singleLineHeight * 1.2f;
        GUILayout.Label("Asset path:");

        filePath = GUILayout.TextField(filePath);
        if (GUILayout.Button("Save"))
        {
            GraphAsset asset = CreateInstance<GraphAsset>();
            if(graph.Save(asset))
            {
                AssetDatabase.CreateAsset(asset, filePath);
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
            }
        }

        if (GUILayout.Button("Load"))
        {
            GraphAsset data = AssetDatabase.LoadAssetAtPath<GraphAsset>(filePath);
            if(data != null)
            {
                Graph<GraphAsset,NodeData,RouteData> newGraph = null;
                if(Graph<GraphAsset,NodeData,RouteData>.Load(data,out newGraph))
                {
                    graph = newGraph;
                }
            }
        }
        GUILayout.EndArea();
    }
}
