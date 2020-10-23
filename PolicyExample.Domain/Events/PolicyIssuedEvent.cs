using System;

namespace PolicyExample.Domain
{
    public class PolicyIssuedEvent : PolicyEvent
    {
        public DateTimeOffset Issued { get; }

        public PolicyIssuedEvent(string source, DateTimeOffset issued) : base(source)
        {
            Issued = issued;
        }
    }
}