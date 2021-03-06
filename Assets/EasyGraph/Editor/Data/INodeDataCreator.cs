using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataCreator<TNodeData>
        where TNodeData : INodeDataWrapper
    {
        NodeDataCreatorEntry<TNodeData> GetCreatorEntry();
    }

}
