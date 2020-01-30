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
        GUI.Label(
        new Rect(rect.position + EditorGUIUtility.singleLineHeight * Vector2.up, new Vector2(rect.width, EditorGUIUtility.singleLineHeight)),
        data.strData);
    }

    public void OnDetailGUI(Rect rect)
    {
        data.strData = GUI.TextField(
        new Rect(rect.position + EditorGUIUtility.singleLineHeight * Vector2.up, new Vector2(rect.width, EditorGUIUtility.singleLineHeight)),
        data.strData);
    }
}
