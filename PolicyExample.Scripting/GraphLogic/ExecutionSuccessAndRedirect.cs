namespace PolicyExample.Scripting.GraphLogic
{
    public class ExecutionSuccessAndRedirect : NodeExecutionResult
    {
        public LogicNode NextNode { get; set; }
    }
}