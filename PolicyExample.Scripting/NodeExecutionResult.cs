namespace PolicyExample.Scripting
{
    public class NodeExecutionResult
    {
        public string CorrelationId { get; }
        public string NodeId { get; }
        public string Message { get; protected set; }
        public ILogicNode[] Trace {get;}
    }
}