using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jint;
using Microsoft.ClearScript.V8;
using PolicyExample.Scripting.Abstractions;
using SlugityLib;

namespace PolicyExample.Scripting.Jint
{

    public class ScriptServiceSet
    {
        private readonly IDictionary<ScriptService, List<ScriptServiceSchema>> _schemes = new Dictionary<ScriptService,List<ScriptServiceSchema>>();
        private readonly IDictionary<ScriptService, object> _services = new Dictionary<ScriptService,object>();

        public void Add(ScriptService descriptor, object service, ScriptServiceSchema? schema = null)
        {
            _services.Add(descriptor,service);
            schema ??= ScriptServiceSchema.Default(descriptor);
            _schemes.Add(descriptor,new List<ScriptServiceSchema>(){schema});
        }

        private bool TryGetDescription(ScriptService service, out ScriptServiceSchema? schema,Language? language = null)
        {
            schema = null;
            var result = _schemes.TryGetValue(service, out var schemas);
            if (result)
            {
                schema = schemas?.FirstOrDefault(s => language == null || s.Language == language);
            }
            return result;
        }

        public bool Contains(IReadOnlyCollection<ScriptService> required)
        {
            return required.All(r => _services.ContainsKey(r));
        }

        public void Inject(Action<IReadOnlyCollection<ScriptServiceSchema>,object> injector, params ScriptService[] required)
        {
            foreach (var requiredService in required)
            {
                if (!_services.TryGetValue(requiredService, out var service))
                    throw new MissingServiceException();
                
                var schemas = _schemes[requiredService];
                 injector.Invoke(schemas, service);
            }
        }
    }

    public class MissingServiceException : Exception
    {
        public MissingServiceException()
        {
        }
    }

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