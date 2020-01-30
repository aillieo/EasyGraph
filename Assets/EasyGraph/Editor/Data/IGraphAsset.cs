using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface IGraphAsset<TData> where TData : INodeDataWrapper
    {
        bool GraphToAsset(Vector2 canvasSize, IList<Node<TData>> nodes, IList<Route<TData>> routes);
        bool AssetToGraph(out Vector2 canvasSize, out IList<Node<TData>> nodes, out IList<Route<TData>> routes);
    }
}
