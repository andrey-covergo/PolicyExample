using System;

namespace PolicyExample.Domain
{
    public class PolicyState : ICloneable
    {
        public bool Issued { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTimeOffset? IssueDate { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset BusinessTime { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}