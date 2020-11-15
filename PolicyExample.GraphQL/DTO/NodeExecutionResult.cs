using System;

namespace PolicyExample.GraphQL.DTO
{
    public interface NodeExecutionResult {
        
        public string NodeID { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
    
        
        public string Output { get; set; }
    }
}