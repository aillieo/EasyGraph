using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class Selection<TNodeData,TRouteData>
        where TNodeData: INodeDataWrapper
        where TRouteData : IRouteDataWrapper,new()
    {
        private readonly List<CanvasElement<TNodeData,TRouteData>> currentSelected = new List<CanvasElement<TNodeData,TRouteData>>();

        public int Select(CanvasElement<TNodeData,TRouteData> element)
        {
            currentSelected.Clear();
            currentSelected.Add(element);
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

        public bool HasSelected(CanvasElement<TNodeData,TRouteData> element)
        {
            return currentSelected.Contains(element);
        }

        public CanvasElement<TNodeData,TRouteData> FirstSelected()
        {
            if(currentSelected.Count == 0)
            {
                return null;
            }
            return currentSelected[0];
        }

    }
}
