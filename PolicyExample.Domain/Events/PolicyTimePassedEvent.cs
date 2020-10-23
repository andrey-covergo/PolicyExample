using System;

namespace PolicyExample.Domain
{
    public class PolicyTimePassedEvent : PolicyEvent
    {
        public DateTimeOffset CurrentTime { get; }

        public PolicyTimePassedEvent(string source, DateTimeOffset currentTime) : base(source)
        {
            CurrentTime = currentTime;
        }
    }
}