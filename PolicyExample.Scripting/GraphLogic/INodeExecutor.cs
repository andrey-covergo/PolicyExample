using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    public interface INodeExecutor
    {
        Task<NodeExecutionResult> Execute(LogicNode node);
        void Validate(LogicNode node);
    }

    public static class NodeExecutorExtensions
    {
        public static void ValidateAll(this INodeExecutor e, LogicNode l)
        {
            foreach(var n in l.GetTree())
                e.Validate(n);
        }
    }
}