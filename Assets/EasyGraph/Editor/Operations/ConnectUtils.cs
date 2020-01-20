using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public static class ConnectUtils
    {
        public static readonly Builder currentBuilder = new Builder();

        public class Builder
        {
            private Node nodeFrom;
            private DummyRoute dummyRoute;

            public bool IsBuilding
            {
                get { return dummyRoute != null; }
            }

            public Route FinishWith(Node node)
            {
                if(node == nodeFrom)
                {
                    Abandon();
                    return null;
                }
                else
                {
                    Route route = new Route(nodeFrom,node);
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
                if(dummyRoute != null)
                {
                    dummyRoute.canvas.RemoveElement(dummyRoute);
                    dummyRoute = null;
                }
                nodeFrom = null;
            }

        }


        public static void RemoveAllRoutes(Node node)
        {
            Route[] routes = new Route[node.associatedRoutes.Count];
            node.associatedRoutes.CopyTo(routes);
            foreach (var r in routes)
            {
                node.canvas.RemoveElement(r);
            }
        }
    }
}
