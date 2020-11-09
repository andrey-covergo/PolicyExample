using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class LogicNodeWithFacade : LogicNode
    {
        private readonly NodeBehaviorFacade _facade;
        public LogicNodeWithFacade()
        {   
            _facade = new NodeBehaviorFacade(this);
        }

        protected virtual void Execute(IExecutionFlow flow, NodeBehaviorFacade facade)
        {
            
        }
        public override Task<NodeExecutionResult> Execute(IExecutionFlow flow)
        {
            Execute(flow, _facade);
            return _facade.Result == null ? base.Execute(flow) : Task.FromResult(_facade.Result);
        }
    }
}