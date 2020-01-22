using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Selection<TData> where TData : INodeDataWrapper
    {
        private readonly List<Node<TData>> currentSelected = new List<Node<TData>>();

        public int Select(Node<TData> node)
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

        public bool HasSelected(Node<TData> node)
        {
            return currentSelected.Contains(node);
        }

        public Node<TData> FirstSelected()
        {
            if(currentSelected.Count == 0)
            {
                return null;
            }
            return currentSelected[0];
        }

    }
}
