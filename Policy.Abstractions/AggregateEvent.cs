using System;

namespace Policy.Abstractions
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
        public new AggregateAddress<T> Source { get; }
    }
}