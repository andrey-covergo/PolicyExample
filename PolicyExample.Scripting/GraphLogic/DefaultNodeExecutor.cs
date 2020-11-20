using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class DefaultNodeExecutor:INodeExecutor
    {
        public Task<NodeExecutionResult> Execute(LogicNode node)
        {
            return  Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }

        public void Validate(LogicNode node)
        {
           
        }
    }
}