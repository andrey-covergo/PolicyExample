using System;

namespace PolicyExample.GraphQL.Types.DTO
{
    public class NodeExecutionError : NodeExecutionResult {
     
        
        public string NodeID { get; set; }
    
        
        public string Error { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
    
        
        public string Output { get; set; }
        
    }
}