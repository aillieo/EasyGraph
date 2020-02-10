using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface IGraphAsset<TNodeData, TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper, new()
    {
        bool GraphToAsset(Vector2 canvasSize,
            IList<NodeDataWithPosition<TNodeData>> nodesToSave,
            IList<RouteDataWithNodeIndex<TRouteData>> routesToSave);

        bool AssetToGraph(out Vector2 canvasSize,
            out IList<NodeDataWithPosition<TNodeData>> nodesLoaded,
            out IList<RouteDataWithNodeIndex<TRouteData>> routesLoaded);
    }

    public interface IGraphAsset<TNodeData> : IGraphAsset<TNodeData,DefaultRouteDataWrapper>
        where TNodeData : INodeDataWrapper
    {}
}
