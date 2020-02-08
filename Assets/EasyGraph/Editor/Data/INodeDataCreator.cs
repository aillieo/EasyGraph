using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataCreator<TNodeData>
        where TNodeData: INodeDataWrapper
    {
        TNodeData Create();

        string MenuName { get; }
    }

}
