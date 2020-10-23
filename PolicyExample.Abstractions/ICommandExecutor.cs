using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolicyExample.Abstractions
{
    public interface ICommandExecutor
    {
        Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command);
    }

  
}