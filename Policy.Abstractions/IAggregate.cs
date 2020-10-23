namespace Policy.Abstractions
{
    public interface IAggregate
    {
        long Version { get; }
        void Apply(IAggregateEvent @event);
    }
}