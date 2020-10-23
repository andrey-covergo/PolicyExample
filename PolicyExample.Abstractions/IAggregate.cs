namespace PolicyExample.Abstractions
{
    public interface IAggregate:ICommandExecutor
    {
        long Version { get; }
        void Apply(IAggregateEvent @event);
        IAggregateAddress Address { get; }
    }
}