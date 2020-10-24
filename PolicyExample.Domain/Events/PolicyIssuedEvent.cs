using System;

namespace PolicyExample.Domain.Events
{
    public class PolicyIssuedEvent : PolicyEvent
    {
        public PolicyIssuedEvent(string source, DateTimeOffset issued) : base(source)
        {
            Issued = issued;
        }

        public DateTimeOffset Issued { get; }
    }
}