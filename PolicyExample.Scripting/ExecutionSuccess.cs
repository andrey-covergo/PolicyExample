using System.Runtime.Serialization;
using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting
{
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