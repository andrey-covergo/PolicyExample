using System;

namespace PolicyExample.Abstractions
{
    public class AggregateEvent:IAggregateEvent
    {
        public AggregateEvent(IAggregateAddress source, string id, DateTimeOffset occured)
        {
            Source = source;
            Id = id;
            Occured = occured;
        }

        public IAggregateAddress Source { get; }
        public string Id { get; }
        public DateTimeOffset Occured { get; }
    }
    
    
    public class AggregateEvent<T> : AggregateEvent
    {
        public AggregateEvent(AggregateAddress<T> source, string id, DateTimeOffset occured) : base(source, id, occured)
        {
            Source = source;
        }

        public AggregateEvent(string source, string? id=null) : this(Address.New<T>(source), id??Guid.NewGuid().ToString(), DateTimeOffset.Now)
        {
            
        }
        public new AggregateAddress<T> Source { get; }
    }
}