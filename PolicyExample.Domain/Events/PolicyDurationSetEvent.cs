using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class PolicyDurationSetEvent : PolicyEvent
    {
        public TimeSpan Duration { get; }

        public PolicyDurationSetEvent(string source, TimeSpan duration) : base(source)
        {
            Duration = duration;
        }
    }
}