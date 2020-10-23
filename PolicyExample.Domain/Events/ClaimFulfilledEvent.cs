using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class ClaimFulfilledEvent:PolicyEvent
    {
        public decimal Amount { get; }

        public ClaimFulfilledEvent(string source, decimal amount) : base(source)
        {
            Amount = amount;
        }
        public ClaimFulfilledEvent(IAggregateAddress source, decimal amount) : this(source.Id, amount)
        {
        }
    }
}