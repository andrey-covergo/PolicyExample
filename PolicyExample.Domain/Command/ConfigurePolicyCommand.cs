using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class ConfigurePolicyCommand : Command<InsurancePolicy>
    {
        public ConfigurePolicyCommand(AggregateAddress<InsurancePolicy> destination, TimeSpan duration, decimal? amount=null) : base(
            destination)
        {
            Duration = duration;
            Amount = amount;
        }
        
        public ConfigurePolicyCommand(AggregateAddress<InsurancePolicy> destination, decimal amount) : base(
            destination)
        {
            Amount = amount;
        }

        public TimeSpan? Duration { get; }
        public decimal? Amount { get; }
    }
}