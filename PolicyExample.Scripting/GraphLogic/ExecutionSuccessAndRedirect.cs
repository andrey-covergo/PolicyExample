namespace PolicyExample.Scripting.GraphLogic
{
    public class ExecutionSuccessAndRedirect : NodeExecutionResult
    {
        public ExecutionSuccessAndRedirect(LogicNode nextNode)
        {
            NextNode = nextNode;
        }

        public LogicNode NextNode { get; }
    }
}