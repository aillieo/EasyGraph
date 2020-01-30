using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataWrapper
    {
        void OnGUI(Rect rect);

        void OnDetailGUI(Rect rect);

        Vector2 Size { get; }
    }

}
