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
        public List<LogicNode> Children { get;  } = new List<LogicNode>();
        public string Name { get; set; }
        public string Id { get; set; }

        public virtual Task<NodeExecutionResult> Execute(IExecutionFlow flow)
        {
            return Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class NodeVisitResult
    {
        public LogicNode Node { get; set; }
        public NodeExecutionResult? Result { get; set; }
        public LogicNode? NextNode { get; set; }

    }
    public interface IExecutionFlow
    {
        Task<NodeVisitResult> Visit(LogicNode node);
    }

    public class OrderedExecutionFlow:IExecutionFlow
    {
        private readonly Stack<LogicNode> _visitHistory = new Stack<LogicNode>();
        public async Task<NodeVisitResult> Visit(LogicNode? node)
        {
            if (node == null)
                return new NodeVisitResult();

            LogicNode? NotVisitedChild()
            {
                return node.Children.FirstOrDefault(c => !_visitHistory.Contains(c));
            }

            if (_visitHistory.Contains(node))
            {
                var notVisitedChild = NotVisitedChild();
                
                if(notVisitedChild != null)
                    return await Visit(notVisitedChild);

                return await Visit(node.Parent);
            }
            
            _visitHistory.Push(node);
            var executionResult = await node.Execute(this);
            
            switch (executionResult)
            {
                case ExecutionSuccessAndContinue cont:
                {

                    var notVisitedChild = node.Children.FirstOrDefault(c => !_visitHistory.Contains(c));
                    
                    if(notVisitedChild != null)
                        return new NodeVisitResult() {NextNode = notVisitedChild, Node = node, Result = cont};
                    
                    return new NodeVisitResult() {NextNode = node.Parent, Node = node, Result = cont};
                }

                case ExecutionSuccessAndRedirect redirect:
                {
                    return new NodeVisitResult() {NextNode = redirect.NextNode, Node = node, Result = redirect};
                }
                case ExecutionSuccessAndStop stop:
                {
                    return new NodeVisitResult() {Node = node, Result = stop};
                }
            }
            throw new NotImplementedException();
        }
    }
    public class LogicGraph
    {
        public LogicNode Root { get; set; }
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
}