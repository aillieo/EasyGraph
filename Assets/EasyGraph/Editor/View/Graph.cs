using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Graph<TData> where TData : INodeDataWrapper
    {
        private readonly Canvas<TData> canvas;

        public Graph(Vector2 size)
        {
            canvas = new Canvas<TData>(size);
        }

        public void OnGUI(Rect viewRect)
        {
            canvas.OnGUI(viewRect);
        }

        public void OnGUINodeDetail(Rect viewRect)
        {
            if(canvas.operation.selection.SelectedCount() > 0)
            {
                Node<TData> node = canvas.operation.selection.FirstSelected();
                if(node.data != null)
                {
                    node.data.OnDetailGUI(viewRect);
                }
            }
        }

        public string Save()
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.AppendFormat("w={0}|h={1}|", canvas.Size.x, canvas.Size.y);
                Dictionary<Node<TData>, int> nodeIndex = new Dictionary<Node<TData>, int>();
                if (canvas.managedElements.ContainsKey(LayerDefine.Node))
                {
                    var nodes = canvas.managedElements[LayerDefine.Node];
                    int index = 0;
                    foreach (var n in nodes)
                    {
                        Node<TData> node = n as Node<TData>;
                        builder.AppendFormat("node={0},{1},{2}|", node.Position.x, node.Position.y, node.data.OnSave());
                        nodeIndex[node] = index++;
                    }
                }
                if (canvas.managedElements.ContainsKey(LayerDefine.Route))
                {
                    var routes = canvas.managedElements[LayerDefine.Route];
                    foreach (var r in routes)
                    {
                        Route<TData> route = r as Route<TData>;
                        builder.AppendFormat("route={0},{1}|", nodeIndex[route.nodeFrom], nodeIndex[route.nodeTo]);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save graph failed\n" + e);
            }
            return builder.ToString();
        }

        public static Graph<TData> Load(string serializedData)
        {
            try
            {
                Vector2 size = Vector2.zero;
                List<Node<TData>> nodes = new List<Node<TData>>();
                List<Route<TData>> routes = new List<Route<TData>>();

                string[] kvs = serializedData.Split('|');
                foreach (var kv in kvs)
                {
                    string[] entry = kv.Split('=');
                    switch (entry[0])
                    {
                        case "w":
                            size.x = float.Parse(entry[1]);
                            break;
                        case "h":
                            size.y = float.Parse(entry[1]);
                            break;
                        case "node":
                            string[] nodeData = entry[1].Split(',');
                            TData data = System.Activator.CreateInstance<TData>();
                            data.OnLoad(nodeData[2]);
                            Node<TData> node = new Node<TData>(data, new Vector2(float.Parse(nodeData[0]), float.Parse(nodeData[1])));
                            nodes.Add(node);
                            break;
                        case "route":
                            string[] routeData = entry[1].Split(',');
                            Route<TData> route = new Route<TData>(nodes[int.Parse(routeData[0])], nodes[int.Parse(routeData[1])]);
                            route.nodeFrom.associatedRoutes.Add(route);
                            route.nodeTo.associatedRoutes.Add(route);
                            routes.Add(route);
                            break;
                    }
                }

                Graph<TData> g = new Graph<TData>(size);
                foreach (var n in nodes)
                {
                    g.canvas.AddElement(n);
                }
                foreach (var r in routes)
                {
                    g.canvas.AddElement(r);
                }

                return g;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load graph failed\n" + e);
            }

            return null;
        }
    }

}
