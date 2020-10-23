namespace PolicyExample.Abstractions
{
    public interface IAggregateFactory
    {
        IAggregate Build(IAggregateAddress address);
    }
}