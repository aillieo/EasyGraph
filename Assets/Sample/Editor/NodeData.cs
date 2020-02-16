using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;
using UnityEditor;

public class NodeData : INodeDataWrapper
{
    public NodeData(StringAndInt stringAndInt)
    {
        this.data = stringAndInt;
    }

    public NodeData()
    {
        this.data = new StringAndInt();
    }

    public readonly StringAndInt data;

    public Vector2 Size => new Vector2(150,80);

    public void OnGUI(Rect rect)
    {
        GUI.Box(rect, GUIContent.none, new GUIStyle("window"));
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        GUILayout.Label(data.strData);
        GUILayout.Label(data.intData.ToString());
    }

    public void OnDetailGUI(Rect rect)
    {
        GUI.Box(rect,GUIContent.none, new GUIStyle("box"));
        GUILayout.Label("String:");
        data.strData = GUILayout.TextField(data.strData);
        GUILayout.Label("Int:");
        data.intData = EditorGUILayout.IntField(data.intData);
    }
}
