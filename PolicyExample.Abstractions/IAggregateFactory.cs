namespace PolicyExample.Abstractions
{
    public interface IAggregateFactory
    {
        IAggregateRoot Build(IAggregateAddress address);
    }
}