using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class ProcessNewTimeCommand : Command<InsurancePolicy>
    {
        public ProcessNewTimeCommand(string policyId, DateTimeOffset newNowTime) : base(policyId)
        {
            NewNowTime = newNowTime;
        }

        public DateTimeOffset NewNowTime { get; }
    }
}