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
        public NodeDataCreatorEntry<TNodeData>[] Entries
        {
            get
            {
                if (entries == null)
                {
                    var creatorTypes1 = System.AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                            .Where(t => t.GetInterfaces().Contains(typeof(INodeDataCreator<TNodeData>))
                                        && !t.IsAbstract));
                    var creatorTypes2 = System.AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes()
                            .Where(t => t.GetInterfaces().Contains(typeof(INodeDataCreators<TNodeData>))
                                        && !t.IsAbstract));
                    List<NodeDataCreatorEntry<TNodeData>> list = creatorTypes1.Select(t =>
                        (Activator.CreateInstance(t) as INodeDataCreator<TNodeData>).GetCreatorEntry()).ToList();
                    creatorTypes2.ToList().ForEach(t =>
                    {
                        list.AddRange((Activator.CreateInstance(t) as INodeDataCreators<TNodeData>).GetCreatorEntries());
                    });

                    list.Sort((a, b) =>
                    {
                        if (a.order != b.order)
                        {
                            return a.order.CompareTo(b.order);
                        }

                        return string.Compare(a.menuName, b.menuName, StringComparison.Ordinal);
                    });

                    entries = list.ToArray();
                }

                return entries;
            }
        }

        private static NodeDataCreatorEntry<TNodeData>[] entries = null;
    }

}
