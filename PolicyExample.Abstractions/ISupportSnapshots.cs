namespace PolicyExample.Abstractions
{
    public interface ISupportSnapshots
    {
        IAggregateRoot RestoreFromSnapshot(ISnapshot snapshot);
        ISnapshot BuildSnapshot();
    }
}