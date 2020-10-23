using System.Collections.Generic;
using System.Threading.Tasks;

namespace Policy.Abstractions
{
    public interface ICommandExecutor
    {
        Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command);
    }
}