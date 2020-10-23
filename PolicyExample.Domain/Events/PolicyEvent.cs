using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{

    public abstract class PolicyEvent : AggregateEvent<Policy>
    {
        public PolicyEvent(string source) : base(source)
        {
        }
    }
}