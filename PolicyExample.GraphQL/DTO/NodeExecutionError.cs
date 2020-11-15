using System;

namespace PolicyExample.GraphQL.DTO
{
    public class NodeExecutionError : NodeExecutionResult {
     
        
        public string NodeID { get; set; }
    
        
        public string Error { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
    
        
        public string Output { get; set; }
        
    }
}