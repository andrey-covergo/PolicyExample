using System.Collections.Generic;

namespace PolicyExample.Scripting.Jint
{
    public class JintScript:IScript
    {
        public JintScript(string javaScriptCode)
        {
            JavaScriptCode = javaScriptCode;
        }

        public string JavaScriptCode { get;  }
        public string Code => JavaScriptCode;
        public Language Language { get; } = Language.JavaScriptEs5;
        public IReadOnlyCollection<ScriptService> RequiredServices { get; set; } = new ScriptService[]{};
    }
}