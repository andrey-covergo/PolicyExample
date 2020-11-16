using System;
using System.Threading.Tasks;
using Jint;
using Microsoft.ClearScript.V8;
using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting.Jint
{
    public class ClearScriptV8Engine:IScriptEngine<ClearScriptScript>, IDisposable
    {
        private readonly V8ScriptEngine _engine;

        public ClearScriptV8Engine()
        {
            _engine = new V8ScriptEngine();
        }
        public Task<IRunResult> Run<T>(ClearScriptScript script, IScriptEnvironment<T> externalEnvironment)
        {
            //TODO: add checks for script context type, version and environment

            var runId = Guid.NewGuid().ToString();
            var jintContext = externalEnvironment.Context;
            _engine.AddHostObject("context", jintContext);
            _engine.Execute(script.JavaScriptCode);
            return Task.FromResult<IRunResult>(new ScriptExecutionSuccess() {Id = runId});
        }

        public void Dispose()
        {
            _engine.Dispose();
        }
    }
}