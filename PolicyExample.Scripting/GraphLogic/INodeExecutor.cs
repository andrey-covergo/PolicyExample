using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public interface INodeExecutor
    {
        Task<NodeExecutionResult> ExecuteNode(LogicNode node);
    }
}