namespace PolicyExample.GraphQL.Types.DTO
{
    /// <summary>
    /// Describes a context available for a script during its execution. Context Schema representation depends on the Engine as well. Schema must be comprehensive for concrete Script language. For Jint engine context schema is a JSON schema
    /// </summary>
    public class ScriptServiceSchema {
     
        
        public string Id { get; set; }
    
        
        public ScriptService Service { get; set; }
    
        
        public string Schema { get; set; }
    
        
        public ScriptEngine Engine { get; set; }
        
    }
}