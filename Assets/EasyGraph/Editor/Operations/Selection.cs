using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Selection
    {
        private readonly List<Node> currentSelected = new List<Node>();

        public int Select(Node node)
        {
            currentSelected.Clear();
            currentSelected.Add(node);
            return currentSelected.Count;
        }

        public void Clear()
        {
            currentSelected.Clear();
        }

        public int SelectedCount()
        {
            return currentSelected.Count;
        }

        public bool HasSelected(Node node)
        {
            return currentSelected.Contains(node);
        }

        public Node FirstSelected()
        {
            if(currentSelected.Count == 0)
            {
                return null;
            }
            return currentSelected[0];
        }

    }
}
