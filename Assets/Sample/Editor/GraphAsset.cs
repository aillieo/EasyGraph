using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AillieoUtils.EasyGraph;

public class GraphAsset : ScriptableObject, IGraphAsset<NodeData,RouteData>
{
    [SerializeField]
    Vector2 canvasSize;

    [SerializeField]
    StringAndInt[] nodeData;

    [SerializeField]
    Vector2[] nodePositions;

    [SerializeField]
    int[] routes;

    public bool AssetToGraph(out Vector2 canvasSize, out IList<Node<NodeData,RouteData>> nodesLoaded, out IList<Route<NodeData,RouteData>> routesLoaded)
    {
        canvasSize = this.canvasSize;
        nodesLoaded = new List<Node<NodeData,RouteData>>();
        routesLoaded = new List<Route<NodeData,RouteData>>();

        for(int i = 0;i<this.nodeData.Length; ++i)
        {
            NodeData data = new NodeData(nodeData[i]);
            Node<NodeData,RouteData> node = new Node<NodeData,RouteData>(data, this.nodePositions[i]);
            nodesLoaded.Add(node);
        }

        for(int i = 0; i < this.routes.Length; i+=2)
        {
            Route<NodeData,RouteData> route = new Route<NodeData,RouteData>(
            nodesLoaded[this.routes[i]],
            nodesLoaded[this.routes[i+1]]);
            routesLoaded.Add(route);
        }

        return true;
    }

    public bool GraphToAsset(Vector2 canvasSize, IList<Node<NodeData,RouteData>> nodesToSave, IList<Route<NodeData,RouteData>> routesToSave)
    {
        this.canvasSize = canvasSize;

        if(nodesToSave == null)
        {
            return true;
        }

        if(routesToSave == null)
        {
            routesToSave = new List<Route<NodeData,RouteData>>();
        }

        Dictionary<Node<NodeData,RouteData>, int> nodeIndex = new Dictionary<Node<NodeData,RouteData>, int>();
        nodeData = new StringAndInt[nodesToSave.Count];
        nodePositions = new Vector2[nodesToSave.Count];
        this.routes = new int[2 * routesToSave.Count];

        for(int i = 0;  i < nodesToSave.Count; i ++)
        {
            var node = nodesToSave[i];
            this.nodeData[i] = node.data.data;
            this.nodePositions[i] = node.Position;
            nodeIndex[node] = i;
        }

        for (int i = 0; i < routesToSave.Count; i++)
        {
            var route = routesToSave[i];
            this.routes[i * 2] = nodeIndex[route.nodeFrom];
            this.routes[i * 2 + 1] = nodeIndex[route.nodeTo];
        }
        return true;
    }
}
