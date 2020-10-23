using System;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class ProcessNewTimeCommand : Command<Policy>
    {
        public DateTimeOffset NewNowTime { get; }

        public ProcessNewTimeCommand(string policyId, DateTimeOffset newNowTime):base(policyId)
        {
            NewNowTime = newNowTime;
        }
    }
}