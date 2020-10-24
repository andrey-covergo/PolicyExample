using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Infrastructure
{
    public class StatelessLocalCommandHandler : ICommandExecutor
    {
        private readonly IEventStore _eventStore;
        private readonly IAggregateFactory _factory;
        private readonly ISnapshotStore _snapshotStore;

        public StatelessLocalCommandHandler(IAggregateFactory factory, IEventStore eventStore,
            ISnapshotStore snapshotStore)
        {
            _factory = factory;
            _snapshotStore = snapshotStore;
            _eventStore = eventStore;
        }

        public async Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command)
        {
            var aggregate = await LoadAggregate(command);

            var events = await aggregate.Execute(command);
            await _eventStore.SaveEvents(command.Id, events);

            //after event persistence we must be tolerant to any snapshot persistence failure
            if (aggregate is ISupportSnapshots currentAggregate)
            {
                var snapshot = currentAggregate.BuildSnapshot();
                await _snapshotStore.SaveSnapshot(snapshot);
            }

            return events;
        }

        private async Task<IAggregateRoot> LoadAggregate(ICommand command)
        {
            var aggregate = _factory.Build(command.Destination);

            if (aggregate is ISupportSnapshots withSnapshots)
            {
                var snapshot = await _snapshotStore.GetSnapshot(command.Destination);
                if (snapshot != null)
                    aggregate = withSnapshots.RestoreFromSnapshot(snapshot);
            }

            await foreach (var e in _eventStore.GetEventStream(aggregate.Address)) aggregate.Apply(e);

            return aggregate;
        }
    }
}