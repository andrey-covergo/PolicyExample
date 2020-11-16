using System;
using System.Threading.Tasks;
using Jint;
using PolicyExample.Scripting.Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    public class JintNodeExecutor:INodeExecutor
    {
        private readonly Engine _engine;

        public JintNodeExecutor()
        {
            _engine = new Engine();
        }
        
        public Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            
            if (node is JintLogicNode jintLogicNode)
            {
                var service = new NodeFlowService(node);
                if (jintLogicNode.Script is JintScript jintScript)
                {
                    try
                    {
                        _engine.SetValue("flow", service);
                        _engine.Execute(jintScript.JavaScriptCode);
                    }
                    catch (Exception ex)
                    {
                        service.Result = new ExecutionError(){Message = ex.ToString()}; 
                    }
                }

                return   service.Result== null ? node.Execute() : Task.FromResult(service.Result);
            }

            return node.Execute();
        }
    }
}