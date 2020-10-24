using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public static class AggregateEventsExtensions
    {
        public static Task<IReadOnlyCollection<IAggregateEvent>> ToResult(this IAggregateEvent e)
        {
            return Task.FromResult<IReadOnlyCollection<IAggregateEvent>>(new[] {e});
        }

        public static Task<IReadOnlyCollection<IAggregateEvent>> ToResult(this IAggregateEvent[] e)
        {
            return Task.FromResult<IReadOnlyCollection<IAggregateEvent>>(e);
        }
    }
}