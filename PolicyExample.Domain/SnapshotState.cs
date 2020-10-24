using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SnapshotState<T> : ISnapshot
    {
        public T State { get; set; }
        public IAggregateAddress Address { get; set; }
        public long Version { get; set; }
        public string Id { get; set; }
    }
}