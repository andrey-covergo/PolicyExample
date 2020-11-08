using System.Threading.Tasks;

namespace PolicyExample.Scripting
{
    /// <summary>
    /// Provides a new context to a script
    /// By the end of script execution Apply() must be called to accept any changes
    /// made in the context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISupportScripting<T>
    {
        IScriptEnvironment<T> CreateEnvironment { get; }
        Task Apply(IScriptEnvironment<T> environment);

    }
}