using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class PolicySnapshot:ISnapshot
    {
        IAggregateAddress ISnapshot.Address => Address;
        public AggregateAddress<InsurancePolicy> Address { get; set; }
        public long Version { get; set; }
        public string Id { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTimeOffset? IssueDate { get; set; }
    }
}