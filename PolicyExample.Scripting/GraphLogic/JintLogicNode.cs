using System;
using System.Threading.Tasks;
using Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    public class JintLogicNode : LogicNodeWithFacade
    {
        public string? JavaScript { get; set; }

        public Task<NodeExecutionResult> Execute(Engine engine)
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

            return base.Execute();
        }
    }
}