using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyGraph
{
    public class NodeDataFactory<TNodeData>
        where TNodeData : INodeDataWrapper
    {
        public INodeDataCreator<TNodeData>[] Creators
        {
            get
            {
                if (creators == null)
                {
                    var creatorTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                            .Where(t => t.GetInterfaces().Contains(typeof(INodeDataCreator<TNodeData>))
                                        && !t.IsAbstract)).ToArray();
                    creators = creatorTypes.Select(t => Activator.CreateInstance(t) as INodeDataCreator<TNodeData>).ToArray();
                }
                return creators;
            }
        }

        private static INodeDataCreator<TNodeData>[] creators = null;

    }

}
