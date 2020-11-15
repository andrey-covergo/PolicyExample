using System.Collections.Generic;

namespace PolicyExample.GraphQL.Types.DTO
{
    public class Script {
        public string Id { get; set; }
        
        public List<ScriptService> RequiredServices { get; set; }
        
        public string Body { get; set; }
    
        public Language Language { get; set; }
    }
}