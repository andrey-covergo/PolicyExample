using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public class CreateLogicNodeResult : CommandExecutionResult {
     
        
        public List<string> Errors { get; set; }
    
        
        public bool Success { get; set; }
    
        
        public string LogicNodeId { get; set; }
        
    }
}