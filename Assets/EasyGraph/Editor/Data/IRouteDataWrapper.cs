using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface IRouteDataWrapper
    {
        bool Selectable { get; }

        Color GetColor();

        void OnDetailGUI(Rect rect);
    }

}
