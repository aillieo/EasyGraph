using System;

namespace AillieoUtils.EasyGraph
{
    public class NodeDataCreatorEntry<TNodeData>
        where TNodeData: INodeDataWrapper
    {
        public readonly string menuName;
        public readonly int order;
        public readonly Func<TNodeData> createFunc;


        public NodeDataCreatorEntry(string menuName, Func<TNodeData> createFunc, int order = 0)
        {
            this.menuName = menuName;
            this.createFunc = createFunc;
            this.order = order;
        }
    }
}
