using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO
{
    public class PolicyExampleQueries {
     
        
        public List<ScriptService> GetSupportedContexts { get; set; }
    
        
        public List<ScriptContextSchema> GetContextSchemas { get; set; }
        
    }
}