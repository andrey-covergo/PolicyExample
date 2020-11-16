using System;
using System.Threading.Tasks;
using Jint;

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
                if (jintLogicNode.JavaScript != null)
                {
                    try
                    {
                        _engine.SetValue("flow", jintLogicNode.Facade.Facade);
                        _engine.Execute(jintLogicNode.JavaScript);
                    }
                    catch (Exception ex)
                    {
                        jintLogicNode.Facade.Result = new ExecutionError(){Message = ex.ToString()}; 
                    }
                }

                return   jintLogicNode.Facade.Result== null ? node.Execute() : Task.FromResult(  jintLogicNode.Facade.Result);
            }

            return node.Execute();
        }
    }
}