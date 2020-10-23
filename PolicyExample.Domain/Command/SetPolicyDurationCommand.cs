using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SetPolicyDurationCommand : Command<Policy>
    {
        public TimeSpan Duration { get; }

        public SetPolicyDurationCommand(AggregateAddress<Policy> destination, TimeSpan duration) : base(destination)
        {
            Duration = duration;
        }
    }
}