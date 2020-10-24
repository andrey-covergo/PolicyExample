using System;
using System.Threading.Tasks;

namespace PolicyExample.Anemic
{
    public class InsurancePolicyService
    {
        public Task CreateNewPolicy(string policyId, TimeSpan duration, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task IssuePolicy(string policyId, TimeSpan duration, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePolicy(string policyId, TimeSpan duration, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task ProcessPolicyClaim(string policyId, TimeSpan duration, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}