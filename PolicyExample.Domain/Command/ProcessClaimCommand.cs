using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class ProcessClaimCommand : Command<Policy>
    {
        public Claim Claim { get; }

        public ProcessClaimCommand(AggregateAddress<Policy> destination, Claim claim) : base(destination)
        {
            Claim = claim;
        }
    }
}