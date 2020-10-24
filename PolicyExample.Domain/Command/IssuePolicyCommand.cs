using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class IssuePolicyCommand : Command<InsurancePolicy>
    {
        public IssuePolicyCommand(AggregateAddress<InsurancePolicy> destination, DateTimeOffset? issueDate = null) :
            base(destination)
        {
            IssueDate = issueDate ?? DateTimeOffset.Now;
        }

        public DateTimeOffset IssueDate { get; }
    }
}