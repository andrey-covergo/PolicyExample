using System;

namespace PolicyExample.Abstractions
{
    public interface IAggregateEvent
    {
        IAggregateAddress Source { get; }
        string Id { get; }
        DateTimeOffset Occured { get; }
    }
}