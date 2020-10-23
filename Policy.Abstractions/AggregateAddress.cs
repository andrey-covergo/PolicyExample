namespace Policy.Abstractions
{
    public class AggregateAddress : IAggregateAddress
    {
        public AggregateAddress(string type, string id)
        {
            Type = type;
            Id = id;
        }

        public string Type { get; }
        public string Id { get; }
    }
    
    public class AggregateAddress<T> : AggregateAddress
    {
        public AggregateAddress(string id) : base(typeof(T).Name, id)
        {
        }
    }

}