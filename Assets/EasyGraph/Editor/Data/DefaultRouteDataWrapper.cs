using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class DefaultRouteDataWrapper : IRouteDataWrapper
    {
        public bool Selectable => false;

        public Color GetColor()
        {
            return Color.white;
        }

        public void OnDetailGUI(Rect rect)
        {}

    }

}
