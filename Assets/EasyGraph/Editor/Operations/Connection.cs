using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Connection<TNodeData,TRouteData>
        where TNodeData : INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        private Node<TNodeData,TRouteData> nodeFrom;
        private DummyRoute<TNodeData,TRouteData> dummyRoute;

        public bool IsBuilding
        {
            get { return dummyRoute != null; }
        }

        public Route<TNodeData,TRouteData> FinishWith(Node<TNodeData,TRouteData> node)
        {
            if (node == nodeFrom)
            {
                Abandon();
                return null;
            }
            else
            {
                Route<TNodeData,TRouteData> route = new Route<TNodeData,TRouteData>(nodeFrom, node);
                node.canvas.AddElement(route);
                CleanUp();
                return route;
            }
        }

        public void Abandon()
        {
            CleanUp();
        }

        public void StartWith(Node<TNodeData,TRouteData> node)
        {
            CleanUp();
            nodeFrom = node;
            dummyRoute = new DummyRoute<TNodeData,TRouteData>(nodeFrom);
            node.canvas.AddElement(dummyRoute);
        }

        private void CleanUp()
        {
            if (dummyRoute != null)
            {
                dummyRoute.canvas.RemoveElement(dummyRoute);
                dummyRoute = null;
            }
            nodeFrom = null;
        }

    }
}
