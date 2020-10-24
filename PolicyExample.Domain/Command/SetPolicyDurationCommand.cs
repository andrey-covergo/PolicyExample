using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class SetPolicyDurationCommand : Command<InsurancePolicy>
    {
        public SetPolicyDurationCommand(AggregateAddress<InsurancePolicy> destination, TimeSpan duration) : base(
            destination)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }
}