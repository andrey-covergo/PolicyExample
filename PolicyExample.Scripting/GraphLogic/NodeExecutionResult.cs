namespace PolicyExample.Scripting.GraphLogic
{
    public class NodeExecutionResult
    {
        public string CorrelationId { get; }
        public string NodeId { get; }
        public string Message { get; protected set; }
        public LogicNode[] Trace {get;}
    }
}