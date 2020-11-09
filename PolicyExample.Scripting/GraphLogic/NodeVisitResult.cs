namespace PolicyExample.Scripting.GraphLogic
{
    public class NodeVisitResult
    {
        public LogicNode Node { get; set; }
        public NodeExecutionResult? Result { get; set; }
        public LogicNode? NextNode { get; set; }

    }
}