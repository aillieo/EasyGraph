using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;
using UnityEditor;

public class NodeData : INodeDataWrapper
{

    public readonly StringAndInt data = new StringAndInt();

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

    public string OnSave()
    {
        return this.data.strData;
    }

    public void OnLoad(string data)
    {
        this.data.strData = data;
    }

}
