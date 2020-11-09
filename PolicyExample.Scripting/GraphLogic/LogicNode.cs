using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyExample.Scripting.GraphLogic
{
    //TODO: find a proper graph library
    public class LogicNode
    {
        public LogicNode Parent { get; set; }
        public List<LogicNode> Children { get;  } = new List<LogicNode>();
        public string Name { get; set; }
        public string Id { get; set; }

        public virtual Task<NodeExecutionResult> Execute(IExecutionFlow flow)
        {
            return Task.FromResult<NodeExecutionResult>(ExecutionSuccessAndContinue.Instance);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}