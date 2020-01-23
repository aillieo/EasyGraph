using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class NodeDataFactory<TData> where TData : INodeDataWrapper
    {
        public INodeDataCreator<TData>[] Creators
        {
            get
            {
                if (creators == null)
                {
                    var creatorTypes = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(INodeDataCreator<TData>)) && !t.IsAbstract)).ToArray();
                    creators = creatorTypes.Select(t => Activator.CreateInstance(t) as INodeDataCreator<TData>).ToArray();
                }
                return creators;
            }
        }

        private static INodeDataCreator<TData>[] creators = null;

    }

}
