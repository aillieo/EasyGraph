using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class RouteDataWithNodeIndex<T> where T : IRouteDataWrapper
    {
        public readonly int fromIndex;
        public readonly int toIndex;
        public readonly T routeData;

        public RouteDataWithNodeIndex(int fromIndex, int toIndex, T routeData)
        {
            this.fromIndex = fromIndex;
            this.toIndex = toIndex;
            this.routeData = routeData;
        }
    }

    public class NodeDataWithPosition<T> where T : INodeDataWrapper
    {
        public readonly Vector2 position;
        public readonly T nodeData;

        public NodeDataWithPosition(Vector2 position, T nodeData)
        {
            this.position = position;
            this.nodeData = nodeData;
        }
    }
}

