using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class IssuePolicyCommand : Command<InsurancePolicy>
    {
        public DateTimeOffset IssueDate { get; }

        public IssuePolicyCommand(AggregateAddress<InsurancePolicy> destination, DateTimeOffset? issueDate=null) : base(destination)
        {
            IssueDate = issueDate??DateTimeOffset.Now;
        }
    }
}