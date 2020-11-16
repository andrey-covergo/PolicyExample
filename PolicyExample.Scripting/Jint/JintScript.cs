using System.Collections.Generic;

namespace PolicyExample.Scripting.Jint
{

    public interface IScript
    {
        string Code { get; }
        Language Language { get; }
        IReadOnlyCollection<ScriptService> RequiredServices { get; }
    }

    public class ScriptService
    {
        public string Name { get; set; }
        public int Version { get; set; }
    }
    public enum Language
    {
        JavaScriptEs5,
        JavaScriptEs6
    }
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