namespace PolicyExample.Scripting.GraphLogic
{
    public interface INodeFlowService
    {
        void Stop();
        void RedirectToParent();
        bool RedirectToChild(int index);
    }
   

    /// <summary>
    /// Exposed to a script, should not have any public members more than available to a script
    /// </summary>
    public class NodeFlowService : INodeFlowService
    {
        
        /// <summary>
        /// Must hide properties and fields from Script
        /// </summary>
        internal NodeExecutionResult? Result { get; set; }
        private readonly LogicNode _node;
        public INodeFlowService Facade => this;
        public NodeFlowService(LogicNode node)
        {
            _node = node;
        }
        public void Stop()
        {
            Result = ExecutionSuccessAndStop.Instance;
        }

        public void RedirectToParent()
        {
            Result = new ExecutionSuccessAndRedirect(_node.Parent);
        }
        
        public bool RedirectToChild(int index)
        {
            if (_node.Children.Count >= index)
            {
                Result = new ExecutionSuccessAndRedirect(_node.Children[index]);
                return true;
            }

            return false;
        }
    }
}