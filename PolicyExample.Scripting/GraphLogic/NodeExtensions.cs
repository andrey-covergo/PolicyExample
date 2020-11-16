using System.Collections.Generic;
using System.Linq;

namespace PolicyExample.Scripting.GraphLogic
{
    public static class NodeExtensions
    {
        public static IEnumerable<LogicNode> GetAllChildrenNodes(this LogicNode node)
        {
            return new[] {node}.Concat(node.Children.SelectMany(c => GetAllChildrenNodes(c)));
        }
    }
}