using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class LogicNodeWithFacade : LogicNode
    {
        protected readonly NodeBehaviorFacade Facade;
        public LogicNodeWithFacade()
        {   
            Facade = new NodeBehaviorFacade(this);
        }
        public override Task<NodeExecutionResult> Execute(IExecutionFlow flow)
        {
            return Facade.Result == null ? base.Execute(flow) : Task.FromResult(Facade.Result);
        }
    }
}