using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public class ProcessClaimCommand : Command<InsurancePolicy>
    {
        public Claim Claim { get; }

        public ProcessClaimCommand(AggregateAddress<InsurancePolicy> destination, Claim claim) : base(destination)
        {
            Claim = claim;
        }
    }
}