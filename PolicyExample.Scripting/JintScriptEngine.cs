using System;
using System.Threading.Tasks;
using Jint;
using PolicyExample.Scripting;

namespace PolicyExample.Tests
{
    public class JintScriptEngine:IScriptEngine<IJintScript>
    {
        private readonly Engine _engine;

        public JintScriptEngine()
        {
            _engine = new Engine();
        }
        public Task<IRunResult> Run<T>(IJintScript script, IScriptEnvironment<T> externalEnvironment)
        {
            var runId = Guid.NewGuid().ToString();
            var jintContext = externalEnvironment.Context;
            _engine.SetValue("context", jintContext);
            _engine.Execute(script.JavaScriptCode);
            var result = _engine.GetCompletionValue().ToObject();
            return Task.FromResult<IRunResult>(new ExecutionSuccess() {Id = runId, Result = result});
        }
    }
}