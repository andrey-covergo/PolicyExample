using System.Collections.Generic;

namespace PolicyExample.GraphQL.Types.DTO
{
    /// <summary>
    /// Defines scripting language and execution runtime. Now supporting only Jint for JavaScript (ECMA 5.1)
    /// </summary>
    public class ScriptEngine {
     
        
        public string Id { get; set; }
    
        
        public string Name { get; set; }
    
        
        public string Version { get; set; }
    
        
        public List<Language> SupportedScriptLanguages { get; set; }
        
    }
}