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
    Color[] routeColors;

    [SerializeField]
    int[] routes;

    public bool AssetToGraph(out Vector2 canvasSize, out IList<NodeDataWithPosition<NodeData>> nodesLoaded, out IList<RouteDataWithNodeIndex<RouteData>> routesLoaded)
    {
        canvasSize = this.canvasSize;
        nodesLoaded = new List<NodeDataWithPosition<NodeData>>();
        routesLoaded = new List<RouteDataWithNodeIndex<RouteData>>();

        for(int i = 0;i<this.nodeData.Length; ++i)
        {
            NodeDataWithPosition<NodeData> data = new NodeDataWithPosition<NodeData>(nodePositions[i], new NodeData(nodeData[i]));
            nodesLoaded.Add(data);
        }

        for(int i = 0; i < this.routeColors.Length; ++i)
        {
            RouteData data = new RouteData();
            data.data = this.routeColors[i];
            RouteDataWithNodeIndex<RouteData> route = new RouteDataWithNodeIndex<RouteData>(routes[i * 2],routes[i * 2 + 1], data);
            routesLoaded.Add(route);
        }

        return true;
    }

    public bool GraphToAsset(Vector2 canvasSize, IList<NodeDataWithPosition<NodeData>> nodesToSave, IList<RouteDataWithNodeIndex<RouteData>> routesToSave)
    {
        this.canvasSize = canvasSize;

        if(nodesToSave == null)
        {
            return true;
        }

        if(routesToSave == null)
        {
            routesToSave = new List<RouteDataWithNodeIndex<RouteData>>();
        }

        nodeData = new StringAndInt[nodesToSave.Count];
        nodePositions = new Vector2[nodesToSave.Count];
        this.routes = new int[2 * routesToSave.Count];
        this.routeColors = new Color[routesToSave.Count];

        for(int i = 0; i < nodesToSave.Count; i ++)
        {
            var node = nodesToSave[i];
            this.nodeData[i] = node.nodeData.data;
            this.nodePositions[i] = node.position;
        }

        for (int i = 0; i < routesToSave.Count; i++)
        {
            var route = routesToSave[i];
            this.routeColors[i] = route.routeData.data;
            this.routes[i * 2] = route.fromIndex;
            this.routes[i * 2 + 1] = route.toIndex;
        }
        return true;
    }
}
