using System;

namespace PolicyExample.Domain
{
    public class PolicyCreatedEvent:PolicyEvent
    {
        public PolicyCreatedEvent(string? source=null) : base(source??Guid.NewGuid().ToString())
        {
        }
    }
}