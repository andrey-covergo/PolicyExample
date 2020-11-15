using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public class PolicyExampleQueries {
     
        
        public List<ScriptService> GetSupportedContexts { get; set; }
    
        
        public List<ScriptServiceSchema> GetContextSchemas { get; set; }
        
    }
}