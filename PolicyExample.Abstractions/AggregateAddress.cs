using System;

namespace PolicyExample.Abstractions
{
    public class AggregateAddress : IAggregateAddress
    {
        public AggregateAddress(string type, string id)
        {
            Type = type;
            Id = id;
        }

        public string Type { get; }
        public string Id { get; }
    }
    
    public class AggregateAddress<T> : AggregateAddress
    {
        public AggregateAddress(string? id=null) : base(typeof(T).Name, id??Guid.NewGuid().ToString())
        {
        }
    }

}