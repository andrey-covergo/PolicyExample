using System;
using System.Collections.Generic;

namespace PolicyExample.GraphQL.Types.DTO
{
    public class RunReport {
     
        
        public string Id { get; set; }
    
        
        public ScriptEngine ScriptEngine { get; set; }
    
        
        public List<NodeExecutionResult> Trace { get; set; }
    
        
        public DateTimeOffset Time { get; set; }
        
    }
}