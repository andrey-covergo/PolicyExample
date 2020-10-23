using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class IssuePolicyCommand : Command<Policy>
    {
        public DateTimeOffset IssueDate { get; }

        public IssuePolicyCommand(AggregateAddress<Policy> destination, DateTimeOffset? issueDate=null) : base(destination)
        {
            IssueDate = issueDate??DateTimeOffset.Now;
        }
    }
}