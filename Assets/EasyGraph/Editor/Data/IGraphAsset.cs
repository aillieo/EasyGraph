using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface IGraphAsset<TNodeData,TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        bool GraphToAsset(Vector2 canvasSize,
            IList<Node<TNodeData,TRouteData>> nodesToSave,
            IList<Route<TNodeData,TRouteData>> routesToSave);
        bool AssetToGraph(out Vector2 canvasSize,
            out IList<Node<TNodeData,TRouteData>> nodesLoaded,
            out IList<Route<TNodeData,TRouteData>> routesLoaded);
    }

    public interface IGraphAsset<TNodeData>
        where TNodeData : INodeDataWrapper
    {
        bool GraphToAsset(Vector2 canvasSize,
            IList<Node<TNodeData,DefaultRouteDataWrapper>> nodesToSave,
            IList<Route<TNodeData,DefaultRouteDataWrapper>> routesToSave);
        bool AssetToGraph(out Vector2 canvasSize,
            out IList<Node<TNodeData,DefaultRouteDataWrapper>> nodesLoaded,
            out IList<Route<TNodeData,DefaultRouteDataWrapper>> routesLoaded);
    }

}
