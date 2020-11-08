using System.Threading.Tasks;

namespace PolicyExample.Scripting
{
    public interface ILogicNode
    {
        ILogicNode Parent { get; }
        ILogicNode[] Children { get; }
        string Name { get; }
        string Id { get; }
        Task<NodeExecutionResult> Execute();

    }
}