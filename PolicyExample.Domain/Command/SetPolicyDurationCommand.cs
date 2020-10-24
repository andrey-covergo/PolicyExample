using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SetPolicyDurationCommand : Command<InsurancePolicy>
    {
        public TimeSpan Duration { get; }

        public SetPolicyDurationCommand(AggregateAddress<InsurancePolicy> destination, TimeSpan duration) : base(destination)
        {
            Duration = duration;
        }
    }
}