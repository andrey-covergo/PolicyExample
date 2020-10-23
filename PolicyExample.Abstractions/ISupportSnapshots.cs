namespace PolicyExample.Abstractions
{
    public interface ISupportSnapshots
    {
        IAggregate RestoreFromSnapshot(ISnapshot snapshot);
        ISnapshot BuildSnapshot();
    }
}