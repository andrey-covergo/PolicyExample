using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Infrastructure
{
    public class EventPublishingCommandHandler : ICommandExecutor
    {
        private readonly ICommandExecutor _executor;
        private readonly IEventPublisher _publisher;

        public EventPublishingCommandHandler(ICommandExecutor executor,IEventPublisher publisher)
        {
            _publisher = publisher;
            _executor = executor;
        }

        public async Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command)
        {
            var events = await _executor.Execute(command);
            await _publisher.Publish(events);
            return events;
        }
    }
}