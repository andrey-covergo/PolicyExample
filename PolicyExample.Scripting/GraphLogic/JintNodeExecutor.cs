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
            if (node.Script?.Language == Language.JavaScriptEs5)
            {
                var service = new NodeFlowService(node);
                try
                {
                    _engine.SetValue("flow", service);
                    _engine.Execute(node.Script.Code);
                }
                catch (Exception ex)
                {
                    service.Result = new ExecutionError(){Message = ex.ToString()}; 
                }
                if(service.Result != null) 
                    return Task.FromResult(service.Result);
            }

            return Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }
    }
}