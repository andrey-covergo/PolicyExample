using PolicyExample.Abstractions;

namespace PolicyExample.Tests
{
    public static class AggregateExtensions
    {
        public static IAggregateRoot Apply(this IAggregateRoot aggregate, params IAggregateEvent[] events)
        {
            foreach (var e in events)
                aggregate.Apply(e);

            return aggregate;
        }

        public static T ApplyEvent<T>(this IAggregateRoot aggregate, T evt) where T : IAggregateEvent
        {
            aggregate.Apply(evt);
            return evt;
        }
    }
}