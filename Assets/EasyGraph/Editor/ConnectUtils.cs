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
            //private Builder() { };

            private Node nodeFrom;
            private Node nodeTo;
            private DummyPath dummyPath;

            public bool IsBuilding
            {
                get { return dummyPath != null; }
            }

            public Path FinishWith(Node node)
            {
                if(node == nodeFrom)
                {
                    Abandon();
                    return null;
                }
                else
                {
                    Path path = new Path(nodeFrom,node);
                    EasyGraphWindow.CurrentCanvas.AddElement(path);
                    CleanUp();
                    return path;
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
                dummyPath = new DummyPath(nodeFrom);
                EasyGraphWindow.CurrentCanvas.AddElement(dummyPath);
            }

            private void CleanUp()
            {
                if(dummyPath != null)
                {
                    EasyGraphWindow.CurrentCanvas.RemoveElement(dummyPath);
                    dummyPath = null;
                }
                nodeFrom = null;
                nodeTo = null;
            }
        }

    }
}
