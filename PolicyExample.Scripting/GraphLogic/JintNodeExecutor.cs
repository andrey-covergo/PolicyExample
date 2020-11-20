using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jint;
using PolicyExample.Scripting.Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    public class JintNodeExecutor:INodeExecutor
    {
        private readonly Engine _engine;
        private readonly ScriptServiceSet _serviceSet;

        public JintNodeExecutor(ScriptServiceSet? serviceSet=null)
        {
            _serviceSet = serviceSet ?? new ScriptServiceSet();
            _engine = new Engine();
        }
        
        public Task<NodeExecutionResult> Execute(LogicNode node)
        {
            if (node.Script?.Language == Language.JavaScriptEs5)
            {
                var service = new NodeFlowService(node);
                _engine.SetValue(KnownServices.ExecutionFlowServiceSchema.AccessName, service);
                
                try
                {
                    _serviceSet.Inject((injectedSchemas, injectedService) =>
                    {
                        var schema = injectedSchemas.FirstOrDefault(_ =>
                            _.Language == Language.JavaScriptEs5);

                        if (schema == null)
                            schema = injectedSchemas.First(_ => _.Language == null);

                        _engine.SetValue(schema.AccessName, injectedService);
                        
                    }, node.Script.RequiredServices.ToArray());

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

        public void Validate(LogicNode node)
        {
            if(node.Script?.RequiredServices!=null && 
             !_serviceSet.Contains(node.Script.RequiredServices))
                throw new MissingServiceException();
        }
    }
}