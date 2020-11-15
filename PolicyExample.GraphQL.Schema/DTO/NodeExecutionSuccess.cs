using System;

namespace PolicyExample.GraphQL.Schema.DTO
{
    public class NodeExecutionSuccess : NodeExecutionResult {
     
        
        public string NodeID { get; set; }
    
        
        public string Content { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
    
        
        public string Output { get; set; }
        
    }
}