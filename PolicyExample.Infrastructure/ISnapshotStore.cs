using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Infrastructure
{
    public interface ISnapshotStore
    {
        Task<ISnapshot?> GetSnapshot(IAggregateAddress address, long? version = null);
        Task SaveSnapshot(ISnapshot snapshot);
    }
}