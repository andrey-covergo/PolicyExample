using System;

namespace Policy.Abstractions
{
    public interface IAggregateEvent
    {
        IAggregateAddress Source { get; }
        string Id { get; }
        DateTimeOffset Occured { get; }
    }
}