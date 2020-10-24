using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{

    public abstract class PolicyEvent : AggregateEvent<InsurancePolicy>
    {
        public PolicyEvent(string source) : base(source)
        {
        }
    }
}