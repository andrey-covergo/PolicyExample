using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public interface IExecutionFlow
    {
        Task<NodeVisitResult> Visit(LogicNode node);
    }
}