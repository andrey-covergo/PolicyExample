using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PolicyExampleAPI;

namespace PolicyExample.API.Web.Controllers
{
    
    public class PolicyControllerLogic:IPolicyController
    {
        public Task<Response<ICollection<PolicyState>>> PolicyGetAsync()
        {
            var state = new PolicyState()
            {
                Amount = 100000,
                BusinessTime = DateTimeOffset.Now,
                Duration = 100,
                Id = "1"
            };
            var response = new Response<ICollection<PolicyState>>(200,new Dictionary<string, IEnumerable<string>>(), new []{state});
            return Task.FromResult(response);
        }

        public Task<Response<string>> PolicyPostAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response<PolicyState>> PolicyGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }
    }

    public class IssuanceControllerLogic : IIssuanceController
    {
        public Task<Response<ICollection<IssuanceWithStatus>>> IssuanceGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<RequestStatus>> IssuancePostAsync(IssuanceRequest body, string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IssuanceWithStatus>> IssuanceGetAsync(string policyId, string issuanceId)
        {
            throw new NotImplementedException();
        }
    }

    public class ConfigurationControllerLogic : IConfigurationController
    {
        public Task<Response<ICollection<ConfigurationWithStatus>>> ConfigurationsGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<RequestStatus>> ConfigurationsPostAsync(Configuration body, string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ConfigurationWithStatus>> ConfigurationsGetAsync(string policyId, string configurationId)
        {
            throw new NotImplementedException();
        }
    }

    public class BusinessTimeControllerLogic : IBusinessTimeController
    {
        public Task<Response<DateTimeOffset>> BusinesstimeGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> BusinesstimePostAsync(DateTimeOffset? body, string policyId)
        {
            throw new NotImplementedException();
        }
    }

    public class ClaimsControllerLogic : IClaimsController
    {
        public Task<Response<ICollection<ClaimWithStatus>>> ClaimGetAsync(string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response> ClaimPostAsync(Claim body, string policyId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ClaimWithStatus>> ClaimGetAsync(string policyId, string claimId)
        {
            throw new NotImplementedException();
        }
    }
}