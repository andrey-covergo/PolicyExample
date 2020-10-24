using System;

namespace PolicyExample.Domain.Events
{
    public class PolicyCreatedEvent : PolicyEvent
    {
        public PolicyCreatedEvent(string? source = null) : base(source ?? Guid.NewGuid().ToString())
        {
        }
    }
}