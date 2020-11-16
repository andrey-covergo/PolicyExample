using System.Threading.Tasks;

namespace PolicyExample.Scripting.Abstractions
{
    public interface IScriptEngine<in TScript>
    {
        public Task<IRunResult> Run<T>(TScript script,IScriptEnvironment<T> externalEnvironment);
    }
}