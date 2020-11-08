using System.Runtime.Serialization;

namespace PolicyExample.Scripting
{
    public class ExecutionSuccessAndStop : NodeExecutionResult
    {
        public ISerializable Result { get; }
    }
}