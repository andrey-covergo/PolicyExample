using System;
using System.Threading.Tasks;
using Jint;
using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting.Jint
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
            //TODO: add checks for script context type, version and environment

            var runId = Guid.NewGuid().ToString();
            var jintContext = externalEnvironment.Context;
            _engine.SetValue("context", jintContext);
            _engine.Execute(script.JavaScriptCode);
            var result = _engine.GetCompletionValue().ToObject();
            return Task.FromResult<IRunResult>(new ExecutionSuccess() {Id = runId, Result = result});
        }
    }
}