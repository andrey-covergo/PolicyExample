using System.Runtime.Serialization;

namespace PolicyExample.Scripting.GraphLogic
{
    public class ExecutionSuccessAndStop : NodeExecutionResult
    {
        public ISerializable Result { get; }
        public static ExecutionSuccessAndStop Instance = new ExecutionSuccessAndStop();
    }
    
}