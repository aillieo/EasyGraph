using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Connection
    {
        private Node nodeFrom;
        private DummyRoute dummyRoute;

        public bool IsBuilding
        {
            get { return dummyRoute != null; }
        }

        public Route FinishWith(Node node)
        {
            if (node == nodeFrom)
            {
                Abandon();
                return null;
            }
            else
            {
                Route route = new Route(nodeFrom, node);
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

        public void StartWith(Node node)
        {
            CleanUp();
            nodeFrom = node;
            dummyRoute = new DummyRoute(nodeFrom);
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
