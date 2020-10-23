using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Infrastructure
{
    public interface IEventPublisher
    {
        Task Publish(IReadOnlyCollection<IAggregateEvent> events);
    }
}