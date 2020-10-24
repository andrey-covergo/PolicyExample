namespace PolicyExample.Abstractions
{
    public interface IAggregateRoot : ICommandExecutor
    {
        long Version { get; }
        IAggregateAddress Address { get; }
        void Apply(IAggregateEvent @event);
    }
}