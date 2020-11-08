using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    
    //TODO: find a proper graph library
    public class LogicNode
    {
        public LogicNode Parent { get; set; }
        public List<LogicNode> Children { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public virtual Task<NodeExecutionResult> Execute(IExecutionFlow flow)
        {
            return Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }
    }

    public class NodeVisitResult
    {
        public LogicNode Node { get; set; }
        public NodeExecutionResult Result { get; set; }
        public LogicNode NextNode { get; set; }
    }
    public interface IExecutionFlow
    {
        LogicNode? CurrentNode { get; }
        Task<NodeVisitResult> Visit(LogicNode node);
    }

    public class SequencedExecutionFlow:IExecutionFlow
    {
        public LogicNode? PreviousNode { get; }
        public async Task<NodeVisitResult> Visit(LogicNode node)
        {
            var result = await node.Execute(this);
            switch (result)
            {
                case ExecutionSuccess _:
                {
                    //sequenced flow
                    if (!node.Children.Any())
                        return new NodeVisitResult() {NextNode = node.Parent, Node = node, Result = result};
                    
                    return new NodeVisitResult() {NextNode = node.Children., Node = node, Result = result};
                }
                case ExecutionSuccessAndRedirect _:
                {
                    //node-decided flow
                    if(node.Children.Any())
                    break;
                }
            }
        }
    }
    public class LogicGraph
    {
        public LogicNode Root { get; set; }
        public IExecutionFlow ExecutionFlow { get; set; }

        IAsyncEnumerable<NodeVisitResult> Run()
        {
            throw new NotImplementedException(); 
        }
    }
}