using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public bool Save(TAsset serializedData)
        {
            try
            {
                IList<Node<TNodeData,TRouteData>> nodes = null;
                IList<Route<TNodeData,TRouteData>> routes = null;
                if (canvas.managedElements.ContainsKey(LayerDefine.Node))
                {
                    nodes = canvas.managedElements[LayerDefine.Node].Select(ele => ele as Node<TNodeData,TRouteData>).ToArray();
                }
                if (canvas.managedElements.ContainsKey(LayerDefine.Route))
                {
                    routes = canvas.managedElements[LayerDefine.Route].Select(ele => ele as Route<TNodeData,TRouteData>).ToArray();
                }
                return serializedData.GraphToAsset(canvas.Size, nodes, routes);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save graph failed\n" + e);
                return false;
            }

        }

        public static bool Load(TAsset serializedData, out Graph<TAsset,TNodeData,TRouteData> graph)
        {
            IList<Node<TNodeData,TRouteData>> nodes = null;
            IList<Route<TNodeData,TRouteData>> routes = null;
            Vector2 size = Vector2.zero;

            graph = null;

            try
            {
                if (serializedData.AssetToGraph(out size, out nodes, out routes))
                {
                    graph = new Graph<TAsset, TNodeData, TRouteData>(size);
                    if(nodes != null)
                    {
                        foreach (var n in nodes)
                        {
                            graph.canvas.AddElement(n);
                        }
                    }
                    if(routes != null)
                    {
                        foreach (var r in routes)
                        {
                            graph.canvas.AddElement(r);
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

    /*
    public class Graph<TAsset,TNodeData> :
        Graph<TAsset, TNodeData, DefaultRouteDataWrapper>
        where TNodeData : INodeDataWrapper
        where TAsset : IGraphAsset<TNodeData,IRouteDataWrapper>
    {
        public Graph(Vector2 size):base(size)
        {
        }

        public static bool Load(TAsset serializedData, out Graph<TData, TAsset> graph)
        {
            Graph<TAsset,TNodeData,DefaultRouteDataWrapper> g = null;
            bool ret = Graph<TAsset,TNodeData>.Load(serializedData, out g);
            graph = g as Graph<TAsset,TNodeData>;
            return ret;
        }
    }
    */
}
