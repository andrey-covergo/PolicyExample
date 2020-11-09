using System.Runtime.Serialization;

namespace PolicyExample.Scripting.GraphLogic
{
    public class ExecutionSuccessAndStop : NodeExecutionResult
    {
        public object Result { get; set; }
        public static ExecutionSuccessAndStop Instance = new ExecutionSuccessAndStop();
    }
    
}