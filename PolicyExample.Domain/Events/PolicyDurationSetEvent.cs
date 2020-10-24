using System;

namespace PolicyExample.Domain.Events
{
    public class PolicyDurationSetEvent : PolicyEvent
    {
        public PolicyDurationSetEvent(string source, TimeSpan duration) : base(source)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }
}