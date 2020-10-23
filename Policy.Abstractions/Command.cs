namespace Policy.Abstractions
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
        public Command(AggregateAddress<T> destination, string id) : base(destination, id)
        {
            Destination = destination;
        }
        
        public new AggregateAddress<T> Destination { get; }
    }
}