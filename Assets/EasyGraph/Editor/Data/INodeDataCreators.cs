using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataCreators<TNodeData>
        where TNodeData : INodeDataWrapper
    {
        NodeDataCreatorEntry<TNodeData>[] GetCreatorEntries();
    }

}
