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
            builder.AppendFormat("w={0}|h={1}|",canvas.Size.x,canvas.Size.y);

            if(canvas.managedElements.ContainsKey(LayerDefine.Node))
            {
                var nodes = canvas.managedElements[LayerDefine.Node];
                foreach (var n in nodes)
                {
                    Node<TData> node = n as Node<TData>;
                    builder.AppendFormat("node={0},{1},{2}|", node.Position.x, node.Position.y, node.data.OnSave());
                }
            }
            return builder.ToString();
        }

        public static Graph<TData> Load(string serializedData)
        {
            try
            {
                Vector2 size = Vector2.zero;
                List<Node<TData>> nodes = new List<Node<TData>>();

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
                    }
                }

                Graph<TData> g = new Graph<TData>(size);
                foreach (var n in nodes)
                {
                    g.canvas.AddElement(n);
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
