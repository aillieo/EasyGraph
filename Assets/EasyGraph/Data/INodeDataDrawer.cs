using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataDrawer<T>
    {
        void OnGUI(Rect rect);
    }

}
