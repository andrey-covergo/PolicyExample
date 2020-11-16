using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class LogicNodeWithFacade : LogicNode
    {
        protected readonly NodeFlowService Facade;
        public LogicNodeWithFacade()
        {   
            Facade = new NodeFlowService(this);
        }
        public override Task<NodeExecutionResult> Execute()
        {
            return Facade.Result == null ? base.Execute() : Task.FromResult(Facade.Result);
        }
    }
}