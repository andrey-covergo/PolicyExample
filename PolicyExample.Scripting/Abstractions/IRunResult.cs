namespace PolicyExample.Scripting.Abstractions
{
    public interface IRunResult
    {
        string Id { get; }
        object Result { get; }
    }
}