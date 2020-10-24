using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class SetPolicyAmountCommand : Command<InsurancePolicy>
    {
        public SetPolicyAmountCommand(AggregateAddress<InsurancePolicy> destination, decimal amount) : base(destination)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}