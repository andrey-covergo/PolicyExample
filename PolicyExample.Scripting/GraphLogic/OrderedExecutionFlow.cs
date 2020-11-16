using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jint;
using Jint.Parser.Ast;

namespace PolicyExample.Scripting.GraphLogic
{


    public class JintLogicNode : LogicNodeWithFacade
    {
        public string? JavaScript { get; set; }

        public Task<NodeExecutionResult> Execute(Engine engine, IExecutionFlow flow)
        {
            if (JavaScript != null)
            {
                try
                {
                    engine.SetValue("flow", Facade.Facade);
                    engine.Execute(JavaScript);
                }
                catch (Exception ex)
                {
                    Facade.Result = new ExecutionError(){Message = ex.ToString()}; 
                }
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

        protected override Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            if (node is JintLogicNode jintLogicNode)
            {
                return jintLogicNode.Execute(_engine, this);
            }
            return base.ExecuteNode(node);
        }
    }
    
    public class OrderedExecutionFlow:IExecutionFlow
    {
        private readonly Stack<LogicNode> _visitHistory = new Stack<LogicNode>();
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
            return await node.Execute(this);
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

    public class UnsupportedNodeExecutionResultException : Exception
    {
        
    }
}