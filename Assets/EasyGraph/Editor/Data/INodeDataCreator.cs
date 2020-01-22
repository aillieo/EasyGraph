using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public interface INodeDataCreator<TData> where TData: INodeDataWrapper
    {
        TData Create();

        string MenuName { get; }
    }

}
