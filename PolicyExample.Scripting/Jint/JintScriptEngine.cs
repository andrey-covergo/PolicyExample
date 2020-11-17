using System;
using System.Threading.Tasks;
using Jint;
using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting.Jint
{
    public class JintScriptEngine:IScriptEngine<JSScript>
    {
        private readonly Engine _engine;

        public JintScriptEngine()
        {
            _engine = new Engine();
        }
        public Task<IRunResult> Run<T>(JSScript script, IScriptEnvironment<T> externalEnvironment)
        {
            //TODO: add checks for script context type, version and environment

            var runId = Guid.NewGuid().ToString();
            var jintContext = externalEnvironment.Context;
            _engine.SetValue("context", jintContext);
            _engine.Execute(script.Code);
            var result = _engine.GetCompletionValue().ToObject();
            return Task.FromResult<IRunResult>(new ScriptExecutionSuccess() {Id = runId, Result = result});
        }
    }
}