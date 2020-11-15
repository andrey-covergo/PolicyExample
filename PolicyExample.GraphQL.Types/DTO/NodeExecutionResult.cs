using System;
using System.Collections.Generic;

namespace PolicyExample.GraphQL.Types.DTO
{
    public class NodeExecutionResult {
        
        public LogicNode Node { get; set; }
        public List<string> Errors { get; set; }
    
        public DateTimeOffset Time { get; set; }
        
        public string Output { get; set; }
    }
}