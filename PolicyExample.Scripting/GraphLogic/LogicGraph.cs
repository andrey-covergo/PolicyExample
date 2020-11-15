using System.Collections.Generic;
using System.Linq;

namespace PolicyExample.Scripting.GraphLogic
{
    public class LogicGraph
    {
        public LogicNode? Root { get; set; }
        public IExecutionFlow ExecutionFlow { get; set; }
        
        public async IAsyncEnumerable<NodeVisitResult> Run()
        {
            var currentNode = Root;
            while (true)
            {
                var res = await ExecutionFlow.Visit(currentNode);

                if (res.NextNode == null && res.Result == null)
                {
                    yield break;
                }

                yield return res;
                currentNode = res.NextNode;
            }
        }
    }

    public static class NodeExtensions
    {
        public static IEnumerable<LogicNode> GetAllChildrenNodes(this LogicNode node)
        {
            return new[] {node}.Concat(node.Children.SelectMany(c => c.GetAllChildrenNodes()));
        }
    }
}