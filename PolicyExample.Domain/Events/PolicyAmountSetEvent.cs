namespace PolicyExample.Domain
{
    public class PolicyAmountSetEvent:PolicyEvent
    {
        public decimal Amount { get; }

        public PolicyAmountSetEvent(string source, decimal amount) : base(source)
        {
            Amount = amount;
        }
    }
}