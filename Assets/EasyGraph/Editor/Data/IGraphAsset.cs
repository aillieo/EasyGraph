using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface IGraphAsset<TData> where TData : INodeDataWrapper
    {
        bool GraphToAsset(Vector2 canvasSize, IList<Node<TData>> nodesToSave, IList<Route<TData>> routesToSave);
        bool AssetToGraph(out Vector2 canvasSize, out IList<Node<TData>> nodesLoaded, out IList<Route<TData>> routesLoaded);
    }
}
