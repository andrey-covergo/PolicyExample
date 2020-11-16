using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jint.Parser.Ast;

namespace PolicyExample.Scripting.GraphLogic
{

    public interface INodeExecutor
    {
        Task<NodeExecutionResult> ExecuteNode(LogicNode node);
    }

    public class DefaultNodeExecutor:INodeExecutor
    {
        public Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            return node.Execute();
        }
    }
    
    public class OrderedExecutionFlow:IExecutionFlow
    {
        public OrderedExecutionFlow(INodeExecutor? nodeExecutor=null)
        {
            _nodeExecutor = nodeExecutor ?? new DefaultNodeExecutor();
        }
        private readonly Stack<LogicNode> _visitHistory = new Stack<LogicNode>();
        private readonly INodeExecutor _nodeExecutor;

        public async Task<NodeVisitResult> Visit(LogicNode? node)
        {
            if (node == null)
            {
                //finishing the flow_
                _visitHistory.Clear();
               return new NodeVisitResult();
            }

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
            
            var executionResult = await ExecuteNode(node);
            
            return ProcessNodeResponse(node, executionResult);
        }

        protected virtual async Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            return await _nodeExecutor.ExecuteNode(node);
        }

        protected NodeVisitResult ProcessNodeResponse(LogicNode node, NodeExecutionResult executionResult)
        {
            switch (executionResult)
            {
                case ExecutionSuccessAndContinue cont:
                {
                    var notVisitedChild = node.Children.FirstOrDefault(c => !_visitHistory.Contains(c));

                    if (notVisitedChild != null)
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
                case ExecutionError error:
                {
                    return new NodeVisitResult() {Node = node, Result = error};
                }
            }

            throw new UnsupportedNodeExecutionResultException();
        }
    }
}