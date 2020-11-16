using System.Collections.Generic;

namespace PolicyExample.Scripting.Jint
{
    public interface IScript
    {
        string Code { get; }
        Language Language { get; }
        IReadOnlyCollection<ScriptService> RequiredServices { get; }
    }
}