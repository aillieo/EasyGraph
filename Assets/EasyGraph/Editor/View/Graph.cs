using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Graph<TAsset,TNodeData,TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
        where TAsset : IGraphAsset<TNodeData,TRouteData>
    {
        private readonly Canvas<TNodeData, TRouteData> canvas;

        public Graph(Vector2 size)
        {
            canvas = new Canvas<TNodeData, TRouteData>(size);
        }

        public void OnGUI(Rect viewRect)
        {
            canvas.OnGUI(viewRect);
        }

        public bool OnGUIDetail(Rect viewRect)
        {
            bool hasSelected = (canvas.operation.selection.SelectedCount() > 0);
            if(hasSelected)
            {
                CanvasElement<TNodeData, TRouteData> element = canvas.operation.selection.FirstSelected();
                if (element != null)
                {
                    bool handled = false;
                    if (!handled)
                    {
                        Node<TNodeData, TRouteData> node = element as Node<TNodeData,TRouteData>;
                        if(node != null && node.data != null)
                        {
                            handled = true;
                            GUILayout.BeginArea(viewRect);
                            node.data.OnDetailGUI(new Rect(Vector2.zero,viewRect.size));
                            GUILayout.EndArea();
                        }
                    }
                    if (!handled)
                    {
                        Route<TNodeData, TRouteData> route = element as Route<TNodeData,TRouteData>;
                        if(route != null && route.data != null)
                        {
                            handled = true;
                            GUILayout.BeginArea(viewRect);
                            route.data.OnDetailGUI(new Rect(Vector2.zero,viewRect.size));
                            GUILayout.EndArea();
                        }
                    }

                    if (!handled)
                    {
                        Debug.LogError("invalid element selected");
                    }
                }
            }
            return hasSelected;
        }

        private Dictionary<Node<TNodeData,TRouteData>, int> indexByNode = new Dictionary<Node<TNodeData, TRouteData>, int>();
        public bool Save(TAsset serializedData)
        {
            indexByNode.Clear();
            try
            {
                IList<NodeDataWithPosition<TNodeData>> nodes = new List<NodeDataWithPosition<TNodeData>>();
                IList<RouteDataWithNodeIndex<TRouteData>> routes = new List<RouteDataWithNodeIndex<TRouteData>>();
                if (canvas.managedElements.ContainsKey(LayerDefine.Node))
                {
                    var nodesOnCanvas = canvas.managedElements[LayerDefine.Node];
                    for (int i = 0; i < nodesOnCanvas.Count; ++i)
                    {
                        var node = nodesOnCanvas[i] as Node<TNodeData, TRouteData>;
                        if (node != null)
                        {
                            nodes.Add(new NodeDataWithPosition<TNodeData>(node.Position, node.data));
                            indexByNode.Add(node, i);
                        }
                    }
                }

                if (canvas.managedElements.ContainsKey(LayerDefine.Route))
                {
                    var routesOnCanvas = canvas.managedElements[LayerDefine.Route];
                    foreach (var ele in routesOnCanvas)
                    {
                        var route = ele as Route<TNodeData, TRouteData>;
                        if (route != null)
                        {
                            routes.Add(new RouteDataWithNodeIndex<TRouteData>(
                                indexByNode[route.nodeFrom],
                                indexByNode[route.nodeTo],
                                route.data));
                        }
                    }
                }

                return serializedData.GraphToAsset(canvas.Size, nodes, routes);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save graph failed\n" + e);
                return false;
            }
        }

        private static List<Node<TNodeData, TRouteData>> nodeByIndex = new List<Node<TNodeData, TRouteData>>();
        public static bool Load(TAsset serializedData, out Graph<TAsset,TNodeData,TRouteData> graph)
        {
            IList<NodeDataWithPosition<TNodeData>> nodes = null;
            IList<RouteDataWithNodeIndex<TRouteData>> routes = null;
            Vector2 size = Vector2.zero;

            graph = null;
            nodeByIndex.Clear();

            try
            {
                if (serializedData.AssetToGraph(out size, out nodes, out routes))
                {
                    graph = new Graph<TAsset, TNodeData, TRouteData>(size);
                    if(nodes != null)
                    {
                        for (int i = 0 ; i< nodes.Count; ++ i)
                        {
                            var node = new Node<TNodeData, TRouteData>(nodes[i].nodeData, nodes[i].position);
                            graph.canvas.AddElement(node);
                            nodeByIndex.Add(node);
                        }
                    }
                    if(routes != null)
                    {
                        foreach (var r in routes)
                        {
                            graph.canvas.AddElement(new Route<TNodeData, TRouteData>(
                                nodeByIndex[r.fromIndex],
                                nodeByIndex[r.toIndex],
                                r.routeData));
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load graph failed\n" + e);
            }

            return graph != null;
        }
    }

    public class Graph<TAsset,TNodeData>
        : Graph<TAsset,TNodeData,DefaultRouteDataWrapper>
        where TNodeData : INodeDataWrapper
        where TAsset : IGraphAsset<TNodeData,DefaultRouteDataWrapper>
    {
        public Graph(Vector2 size) : base(size)
        {}

        public static bool Load(TAsset serializedData, out Graph<TAsset,TNodeData> graph)
        {
            Graph<TAsset, TNodeData, DefaultRouteDataWrapper> baseGraph = null;
            bool success = Graph<TAsset, TNodeData, DefaultRouteDataWrapper>.Load(serializedData, out baseGraph);
            graph = baseGraph as Graph<TAsset, TNodeData>;
            return success;
        }
    }
}
