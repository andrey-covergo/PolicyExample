using System.Runtime.Serialization;
using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting
{
    public class ScriptExecutionSuccess:IRunResult
    {
        public string Id { get; set; }
        public object Result { get; set; }
        
    }
}