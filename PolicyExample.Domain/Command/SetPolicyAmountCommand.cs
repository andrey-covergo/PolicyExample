using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SetPolicyAmountCommand : Command<InsurancePolicy>
    {
        public decimal Amount { get; }

        public SetPolicyAmountCommand(AggregateAddress<InsurancePolicy> destination, decimal amount) : base(destination)
        {
            Amount = amount;
        }
    }
   
}