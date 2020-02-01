using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Graph<TData,TAsset> where TData : INodeDataWrapper where TAsset : IGraphAsset<TData>
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
                    GUILayout.BeginArea(viewRect);
                    node.data.OnDetailGUI(new Rect(Vector2.zero,viewRect.size));
                    GUILayout.EndArea();
                }
            }
        }

        public bool Save(TAsset serializedData)
        {
            try
            {
                IList<Node<TData>> nodes = null;
                IList<Route<TData>> routes = null;
                if (canvas.managedElements.ContainsKey(LayerDefine.Node))
                {
                    nodes = canvas.managedElements[LayerDefine.Node].Select(ele => ele as Node<TData>).ToArray();
                }
                if (canvas.managedElements.ContainsKey(LayerDefine.Route))
                {
                    routes = canvas.managedElements[LayerDefine.Route].Select(ele => ele as Route<TData>).ToArray();
                }
                return serializedData.GraphToAsset(canvas.Size, nodes, routes);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save graph failed\n" + e);
                return false;
            }

        }

        public static bool Load(TAsset serializedData, out Graph<TData, TAsset> graph)
        {
            IList<Node<TData>> nodes = null;
            IList<Route<TData>> routes = null;
            Vector2 size = Vector2.zero;

            graph = null;

            try
            {
                if (serializedData.AssetToGraph(out size, out nodes, out routes))
                {
                    graph = new Graph<TData, TAsset>(size);
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

}
