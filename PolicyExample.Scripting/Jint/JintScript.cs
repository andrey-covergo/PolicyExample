using System.Collections.Generic;

namespace PolicyExample.Scripting.Jint
{
    public class JSScript:IScript
    {
        public JSScript(string javaScriptCode, Language? language=null, params ScriptService[] services)
        {
            Code = javaScriptCode;
            Language = language ?? Language.JavaScriptEs5;
            RequiredServices = services;
        }

        public string Code { get; }
        public Language Language { get; } 
        public IReadOnlyCollection<ScriptService> RequiredServices { get; }
    }
}