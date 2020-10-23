namespace Policy.Abstractions
{
    public interface IAggregateAddress
    {
        string Type { get; }
        string Id { get; }
    }
}