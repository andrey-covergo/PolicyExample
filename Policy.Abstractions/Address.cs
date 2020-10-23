namespace Policy.Abstractions
{
    public static class Address
    {
        public static AggregateAddress<T> New<T>(string id)
        {
            return new AggregateAddress<T>(id);
        }
    }
}