namespace PolicyExample.Scripting
{
    public class ExecutionSuccessAndContinue : NodeExecutionResult
    {
        public ILogicNode NextNode { get; }
    }
}