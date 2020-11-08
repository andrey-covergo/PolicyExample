using System.Runtime.Serialization;

namespace PolicyExample.Scripting
{
    public interface IRunResult
    {
        string Id { get; }
        object Result { get; }
    }
}