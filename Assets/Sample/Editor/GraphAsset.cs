using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;

public class GraphAsset : ScriptableObject, IGraphAsset<NodeData>
{
    [SerializeField]
    Vector2 canvasSize;

    [SerializeField]
    StringAndInt[] nodeData;

    [SerializeField]
    Vector2[] nodePositions;

    [SerializeField]
    int[] routes;

    public bool AssetToGraph(out Vector2 canvasSize, out IList<Node<NodeData>> nodes, out IList<Route<NodeData>> routes)
    {
        canvasSize = this.canvasSize;
        nodes = new List<Node<NodeData>>();
        routes = new List<Route<NodeData>>();

        for(int i = 0;i<this.nodeData.Length; ++i)
        {
            NodeData data = new NodeData(nodeData[i]);
            Node<NodeData> node = new Node<NodeData>(data, this.nodePositions[i]);
            nodes.Add(node);
        }

        for(int i = 0; i < this.routes.Length; i+=2)
        {
            Route<NodeData> route = new Route<NodeData>(
            nodes[this.routes[i]], 
            nodes[this.routes[i+1]]);
            routes.Add(route);
        }

        return true;
    }

    public bool GraphToAsset(Vector2 canvasSize, IList<Node<NodeData>> nodes, IList<Route<NodeData>> routes)
    {
        this.canvasSize = canvasSize;

        Dictionary<Node<NodeData>, int> nodeIndex = new Dictionary<Node<NodeData>, int>();
        nodeData = new StringAndInt[nodes.Count];
        nodePositions = new Vector2[nodes.Count];
        this.routes = new int[2 * routes.Count];

        for(int i = 0;  i < nodes.Count; i ++)
        {
            var node = nodes[i];
            this.nodeData[i] = node.data.data;
            this.nodePositions[i] = node.Position;
            nodeIndex[node] = i;
        }

        for (int i = 0; i < routes.Count; i++)
        {
            var route = routes[i];
            this.routes[i * 2] = nodeIndex[route.nodeFrom];
            this.routes[i * 2 + 1] = nodeIndex[route.nodeTo];
        }
        return true;
    }
}
