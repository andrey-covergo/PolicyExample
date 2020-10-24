namespace PolicyExample.Domain.Events
{
    public class PolicyAmountSetEvent : PolicyEvent
    {
        public PolicyAmountSetEvent(string source, decimal amount) : base(source)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}