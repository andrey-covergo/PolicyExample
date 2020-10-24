using System;

namespace PolicyExample.Abstractions
{
    public interface IAggregateAddress:IEquatable<IAggregateAddress>
    {
        string Type { get; }
        string Id { get; }
    }
}