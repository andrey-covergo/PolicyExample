using System.Threading.Tasks;

namespace PolicyExample.Scripting
{
    public interface IScriptEngine<in TScript> where TScript:IScript
    {
        public Task<IRunResult> Run<T>(TScript script,IScriptEnvironment<T> externalEnvironment); //return result 
    }
}