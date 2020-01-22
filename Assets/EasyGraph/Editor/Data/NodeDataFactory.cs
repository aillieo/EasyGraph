using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class NodeDataFactory<TData> where TData : INodeDataWrapper
    {
        public readonly INodeDataCreator<TData>[] creators;

        public NodeDataFactory()
        {
            var creatorTypes = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(INodeDataCreator<TData>)) && !t.IsAbstract)).ToArray();
            creators = creatorTypes.Select(t => Activator.CreateInstance(t) as INodeDataCreator<TData>).ToArray();
        }
    }

}
