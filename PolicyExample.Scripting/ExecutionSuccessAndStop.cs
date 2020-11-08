using System.Runtime.Serialization;

namespace PolicyExample.Scripting
{
    public class ExecutionSuccessAndStop : NodeExecutionResult
    {
        public ISerializable Result { get; }
    }
    
    public class ExecutionSuccess:IRunResult
    {
        public string Id { get; set; }
        public object Result { get; set; }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}