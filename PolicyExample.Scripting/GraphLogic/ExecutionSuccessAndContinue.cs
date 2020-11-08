namespace PolicyExample.Scripting.GraphLogic
{
    public class ExecutionSuccessAndContinue : NodeExecutionResult
    {
        public LogicNode NextNode { get; }
        public static ExecutionSuccessAndContinue Instance = new ExecutionSuccessAndContinue();
    }
}