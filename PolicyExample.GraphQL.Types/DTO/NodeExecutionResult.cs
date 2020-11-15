using System;

namespace PolicyExample.GraphQL.Types.DTO
{
    public interface NodeExecutionResult {
        
        public string NodeID { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
    
        
        public string Output { get; set; }
    }
}