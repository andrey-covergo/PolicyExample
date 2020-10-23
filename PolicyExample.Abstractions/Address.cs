namespace PolicyExample.Abstractions
{
    public static class Address
    {
        public static AggregateAddress<T> New<T>(string? id=null)
        {
            return new AggregateAddress<T>(id);
        }
    }
}