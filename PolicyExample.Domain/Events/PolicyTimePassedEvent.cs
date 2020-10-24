using System;

namespace PolicyExample.Domain.Events
{
    public class PolicyTimePassedEvent : PolicyEvent
    {
        public PolicyTimePassedEvent(string source, DateTimeOffset currentTime) : base(source)
        {
            CurrentTime = currentTime;
        }

        public DateTimeOffset CurrentTime { get; }
    }
}