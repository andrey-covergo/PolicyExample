using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Infrastructure
{
    public interface IEventStore
    {
        IAsyncEnumerable<IAggregateEvent> GetEventStream(IAggregateAddress address);
        Task SaveEvents(string transactionId, IReadOnlyCollection<IAggregateEvent> events);
    }
}