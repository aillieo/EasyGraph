using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;
using UnityEditor;

public class RouteData : IRouteDataWrapper
{
    public RouteData()
    {
        this.data = Color.white;
    }

    public Color data { get; set; }

    public bool Selectable => true;

    public Color GetColor()
    {
        return this.data;
    }

    public void OnDetailGUI(Rect rect)
    {
        this.data = EditorGUILayout.ColorField(this.data);
    }
}
