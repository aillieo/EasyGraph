using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class OperationState<TData> where TData : INodeDataWrapper
    {
        public readonly Connection<TData> connection = new Connection<TData>();

        public readonly Selection<TData> selection = new Selection<TData>();

    }

}
