using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
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
}