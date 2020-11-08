using PolicyExample.Scripting.Abstractions;

namespace PolicyExample.Scripting.Jint
{
    public interface IJintScript : IScript
    {

        string JavaScriptCode { get; }
    }
}