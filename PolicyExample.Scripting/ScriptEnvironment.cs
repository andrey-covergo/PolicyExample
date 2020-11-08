using PolicyExample.Scripting;

namespace PolicyExample.Tests
{
    public class ScriptEnvironment<T>:IScriptEnvironment<T>
    {
        public ScriptEnvironment(string engineVersion, string contextVersion, T context)
        {
            EngineVersion = engineVersion;
            ContextVersion = contextVersion;
            Context = context;
        }

        public string EngineVersion { get; }
        public string ContextVersion { get; }
        public T Context { get; }
    }
}