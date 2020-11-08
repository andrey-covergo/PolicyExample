using PolicyExample.Scripting;

namespace PolicyExample.Tests
{
    public interface IJintScript : IScript
    {
        string JavaScriptCode { get; }
    }
}