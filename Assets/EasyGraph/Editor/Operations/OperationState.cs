using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class OperationState<TNodeData,TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        public readonly Connection<TNodeData,TRouteData> connection = new Connection<TNodeData,TRouteData>();

        public readonly Selection<TNodeData,TRouteData> selection = new Selection<TNodeData,TRouteData>();

    }

}
