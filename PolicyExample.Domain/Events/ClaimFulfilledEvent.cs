using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Events
{
    public class ClaimFulfilledEvent : PolicyEvent
    {
        public ClaimFulfilledEvent(string source, decimal amount) : base(source)
        {
            Amount = amount;
        }

        public ClaimFulfilledEvent(IAggregateAddress source, decimal amount) : this(source.Id, amount)
        {
        }

        public decimal Amount { get; }
    }
}