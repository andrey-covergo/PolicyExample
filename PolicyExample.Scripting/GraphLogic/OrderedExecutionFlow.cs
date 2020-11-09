using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jint;

namespace PolicyExample.Scripting.GraphLogic
{


    public class JintLogicNode : LogicNodeWithFacade
    {
        public string? JavaScript { get; set; }

        public Task<NodeExecutionResult> Execute(Engine engine, IExecutionFlow flow)
        {
            if (JavaScript != null)
            {
                engine.SetValue("flow", Facade);
                engine.Execute(JavaScript);
            }

            return base.Execute(flow);
        }
    }
    public class JintOrderedExecutionFlow : OrderedExecutionFlow
    {
        private readonly Engine _engine;

        public JintOrderedExecutionFlow()
        {
            _engine = new Engine();
        }

        public override async Task<NodeVisitResult> Visit(LogicNode? node)
        {
            if (node is JintLogicNode jintLogicNode)
            {
                VisitHistory.Push(jintLogicNode);
                var nodeResult = await jintLogicNode.Execute(_engine, this);

                return ProcessNodeResponse(node,nodeResult);
            }

            return await base.Visit(node);
        }
    }
    
    public class OrderedExecutionFlow:IExecutionFlow
    {
        protected readonly Stack<LogicNode> VisitHistory = new Stack<LogicNode>();
        public virtual async Task<NodeVisitResult> Visit(LogicNode? node)
        {
            if (node == null)
                return new NodeVisitResult();

            LogicNode? NotVisitedChild()
            {
                return node.Children.FirstOrDefault(c => !VisitHistory.Contains(c));
            }

            if (VisitHistory.Contains(node))
            {
                var notVisitedChild = NotVisitedChild();
                
                if(notVisitedChild != null)
                    return await Visit(notVisitedChild);

                return await Visit(node.Parent);
            }
            
            VisitHistory.Push(node);
            var executionResult = await node.Execute(this);
            
            return ProcessNodeResponse(node, executionResult);
        }

        protected NodeVisitResult ProcessNodeResponse(LogicNode node, NodeExecutionResult executionResult)
        {
            switch (executionResult)
            {
                case ExecutionSuccessAndContinue cont:
                {
                    var notVisitedChild = node.Children.FirstOrDefault(c => !VisitHistory.Contains(c));

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
            }

            throw new NotImplementedException();
        }
    }
}