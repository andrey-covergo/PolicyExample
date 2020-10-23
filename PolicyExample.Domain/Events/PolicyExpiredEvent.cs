namespace PolicyExample.Domain
{
    public class PolicyExpiredEvent:PolicyEvent
    {
        public PolicyExpiredEvent(string id):base(id)
        {
        }
    }
}