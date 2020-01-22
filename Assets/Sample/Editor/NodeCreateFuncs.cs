using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;

public class NodeCreateFuncHello : INodeDataCreator<NodeData>
{
    public string MenuName => "CreateStringHello";

    public NodeData Create()
    {
        NodeData nodeData = new NodeData();
        nodeData.data.strData = "Hello";
        return nodeData;
    }
}

public class NodeCreateFuncWorld : INodeDataCreator<NodeData>
{
    public string MenuName => "CreateStringWorld";

    public NodeData Create()
    {
        NodeData nodeData = new NodeData();
        nodeData.data.strData = "World";
        return nodeData;
    }
}
