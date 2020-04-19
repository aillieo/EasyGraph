using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;

public class NodeCreateFuncHello : INodeDataCreator<NodeData>
{
    private NodeData CreateNode()
    {
        NodeData nodeData = new NodeData();
        nodeData.data.strData = "Hello";
        return nodeData;
    }

    public NodeDataCreatorEntry<NodeData> GetCreatorEntry()
    {
        return new NodeDataCreatorEntry<NodeData>("CreateStringHello", CreateNode);
    }
}

public class NodeCreateFuncWorld : INodeDataCreator<NodeData>
{
    private NodeData CreateNode()
    {
        NodeData nodeData = new NodeData();
        nodeData.data.strData = "World";
        return nodeData;
    }

    public NodeDataCreatorEntry<NodeData> GetCreatorEntry()
    {
        return new NodeDataCreatorEntry<NodeData>("CreateStringWorld", CreateNode);
    }
}
