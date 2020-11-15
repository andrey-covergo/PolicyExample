using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public interface CommandExecutionResult {
        
        public List<string> Errors { get; set; }
    
        
        public bool Success { get; set; }
    }
}