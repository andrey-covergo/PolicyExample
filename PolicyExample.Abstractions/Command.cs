using System;

namespace PolicyExample.Abstractions
{
    public abstract class Command : ICommand
    {
        protected Command(IAggregateAddress destination, string id)
        {
            Destination = destination;
            Id = id;
        }

        public IAggregateAddress Destination { get; }
        public string Id { get; }
    }
    
    public class Command<T> : Command
    {
        public Command(AggregateAddress<T> destination, string? id=null) : base(destination, id??Guid.NewGuid().ToString())
        {
            Destination = destination;
        }
        
        public Command(string destinationId, string? id=null) : this(new AggregateAddress<T>(id), id??Guid.NewGuid().ToString())
        {
        }
        
        public new AggregateAddress<T> Destination { get; }
    }
}