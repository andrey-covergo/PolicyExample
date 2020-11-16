using System.Threading.Tasks;
using Jint;

namespace PolicyExample.Scripting.GraphLogic
{
    public class JintNodeExecutor:INodeExecutor
    {
        private readonly Engine _engine;

        public JintNodeExecutor()
        {
            _engine = new Engine();
        }
        
        public Task<NodeExecutionResult> ExecuteNode(LogicNode node)
        {
            if (node is JintLogicNode jintLogicNode)
            {
                return jintLogicNode.Execute(_engine);
            }

            return node.Execute();
        }
    }
}