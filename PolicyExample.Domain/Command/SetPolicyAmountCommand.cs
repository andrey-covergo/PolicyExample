using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SetPolicyAmountCommand : Command<Policy>
    {
        public decimal Amount { get; }

        public SetPolicyAmountCommand(AggregateAddress<Policy> destination, decimal amount) : base(destination)
        {
            Amount = amount;
        }
    }
   
}