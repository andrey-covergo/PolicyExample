using System;
using PolicyExample.Domain.Events;

namespace PolicyExample.Tests
{
    public class IssuePolicyScriptSetEvent : PolicyEvent
    {
        public IssuePolicyScriptSetEvent(string source, string script) : base(source)
        {
            throw new NotImplementedException();
        }
    }
}