using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Events
{
    public abstract class PolicyEvent : AggregateEvent<InsurancePolicy>
    {
        public PolicyEvent(string source) : base(source)
        {
        }
    }
}