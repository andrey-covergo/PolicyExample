using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class SnapshotState<T> : ISnapshot
    {
        public IAggregateAddress Address { get; set; }
        public long Version { get; set; }
        public string Id { get; set; }
        public T State { get; set; }

    }
    public class PolicyState: ICloneable
    {
        public bool Issued { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTimeOffset? IssueDate { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset BusinessTime{ get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}