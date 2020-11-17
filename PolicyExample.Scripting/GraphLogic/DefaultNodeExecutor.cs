using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public class DefaultNodeExecutor:INodeExecutor
    {
        public Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            return  Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }
    }
}