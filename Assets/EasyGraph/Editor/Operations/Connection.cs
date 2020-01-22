using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Connection<TData> where TData : INodeDataWrapper
    {
        private Node<TData> nodeFrom;
        private DummyRoute<TData> dummyRoute;

        public bool IsBuilding
        {
            get { return dummyRoute != null; }
        }

        public Route<TData> FinishWith(Node<TData> node)
        {
            if (node == nodeFrom)
            {
                Abandon();
                return null;
            }
            else
            {
                Route<TData> route = new Route<TData>(nodeFrom, node);
                node.canvas.AddElement(route);
                nodeFrom.associatedRoutes.Add(route);
                node.associatedRoutes.Add(route);
                CleanUp();
                return route;
            }
        }

        public void Abandon()
        {
            CleanUp();
        }

        public void StartWith(Node<TData> node)
        {
            CleanUp();
            nodeFrom = node;
            dummyRoute = new DummyRoute<TData>(nodeFrom);
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
