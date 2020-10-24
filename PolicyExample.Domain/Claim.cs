namespace PolicyExample.Domain
{
    public class Claim
    {
        public Claim(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}