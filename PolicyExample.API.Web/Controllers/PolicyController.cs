using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyExampleAPI;

namespace PolicyExample.API.Web.Controllers
{
    public class PolicyControllerLogic:IPolicyController
    {
        public Task<ICollection<PolicyState>> PolicyGetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> PolicyPostAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PolicyState> PolicyGetAsync(string policyId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class IssuanceControllerLogic : IIssuanceController
    {
        public Task<ICollection<Anonymous>> IssuanceGetAsync(string policyId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RequestStatus> IssuancePostAsync(IssuanceRequest body, string policyId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RequestStatus> IssuanceGetAsync(string policyId, string issuanceId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ConfigurationControllerLogic : IConfigurationController
    {
        public Task<ICollection<Anonymous2>> ConfigurationsGetAsync(string policyId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RequestStatus> ConfigurationsPostAsync(Configuration body, string policyId)
        {
            throw new System.NotImplementedException();
        }

        public Task<RequestStatus> ConfigurationsGetAsync(string policyId, string configurationId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class BusinessTimeControllerLogic : IBusinessTimeController
    {
        public Task<DateTimeOffset> BusinesstimeGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task BusinesstimePostAsync(DateTimeOffset? body, string policyId)
        {
            throw new NotImplementedException();
        }
    }

    public class ClaimsControllerLogic : IClaimsController
    {
        public Task<ICollection<Claim>> ClaimGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task ClaimPostAsync(Claim body, string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> ClaimGetAsync(string policyId, string claimId)
        {
            throw new NotImplementedException();
        }
    }
}