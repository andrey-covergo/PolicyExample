namespace PolicyExample.Domain.Events
{
    public class PolicyExpiredEvent : PolicyEvent
    {
        public PolicyExpiredEvent(string id) : base(id)
        {
        }
    }
}