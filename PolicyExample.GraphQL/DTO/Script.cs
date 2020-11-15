using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public class Script {
     
        
        public string Id { get; set; }
    
        
        public List<ScriptContext> RequiredContexts { get; set; }
    
        
        public string Body { get; set; }
    
        
        public string Language { get; set; }
        
    }
}