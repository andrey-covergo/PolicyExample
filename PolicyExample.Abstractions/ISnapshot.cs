namespace PolicyExample.Abstractions
{
    public interface ISnapshot
    {
        IAggregateAddress Address { get; }
        long Version { get; }
        string Id { get; }
    }
}