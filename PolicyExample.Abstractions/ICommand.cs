namespace PolicyExample.Abstractions
{
    public interface ICommand
    {
        public IAggregateAddress Destination { get; }
        public string Id { get; }
    }
}