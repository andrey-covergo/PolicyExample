using PolicyExample.Abstractions;

namespace PolicyExample.Domain.Command
{
    public class ProcessClaimCommand : Command<InsurancePolicy>
    {
        public ProcessClaimCommand(AggregateAddress<InsurancePolicy> destination, Claim claim) : base(destination)
        {
            Claim = claim;
        }

        public Claim Claim { get; }
    }
}