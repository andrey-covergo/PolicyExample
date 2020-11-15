using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PolicyExampleAPI;

namespace PolicyExample.API.Rest.Controllers
{
    public static class ResponseCodes
    {
        public static Response<T> OK<T>(T value, Dictionary<string, IEnumerable<string>>? headers=null)
        {
            return new Response<T>(200,headers ?? new Dictionary<string, IEnumerable<string>>(), value);
        }
        
        public static Response<T> Accepted<T>(T value, Dictionary<string, IEnumerable<string>>? headers=null)
        {
            return new Response<T>(202,headers ?? new Dictionary<string, IEnumerable<string>>(), value);
        }
        public static Response Accepted(Dictionary<string, IEnumerable<string>>? headers=null)
        {
            return new Response(202,headers ?? new Dictionary<string, IEnumerable<string>>());
        }

        public static Response OK(Dictionary<string, IEnumerable<string>>? headers=null)
        {
            return new Response(200,headers ?? new Dictionary<string, IEnumerable<string>>());
        }
    }
    
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
            var response = new Response<ICollection<PolicyState>>(200,new Dictionary<string, IEnumerable<string>>
            {
                {"Access-Control-Allow-Origin", new []{"*"}}
            }, new []{state});
            return Task.FromResult(response);
        }

        public Task<Response<string>> PolicyPostAsync()
        {
            return Task.FromResult(ResponseCodes.OK(Guid.NewGuid().ToString()));
        }

        public Task<Response<PolicyState>> PolicyGetAsync(string policyId)
        {
            var state = new PolicyState()
            {
                Amount = 100000,
                BusinessTime = DateTimeOffset.Now,
                Duration = 100,
                Id = policyId
            };
            return Task.FromResult(ResponseCodes.OK(state));
        }
    }

    public class IssuanceControllerLogic : IIssuanceController
    {
        public Task<Response<ICollection<IssuanceWithStatus>>> IssuanceGetAsync(string policyId)
        {
            var issuance = new IssuanceWithStatus()
            {
                Issuance = new IssuanceRequest() {IssueTime = DateTimeOffset.Now, PolicyId = policyId},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            ICollection < IssuanceWithStatus > issuances = new[] {issuance};
            return Task.FromResult(ResponseCodes.OK(issuances));
        }

        public Task<Response<RequestStatus>> IssuancePostAsync(IssuanceRequest body, string policyId)
        {
            var status = new RequestStatus {Id = "", Status = RequestState.Completed};
            return Task.FromResult(ResponseCodes.OK(status));
        }

        public Task<Response<IssuanceWithStatus>> IssuanceGetAsync(string policyId, string issuanceId)
        {
            var issuance = new IssuanceWithStatus()
            {
                Issuance = new IssuanceRequest() {IssueTime = DateTimeOffset.Now, PolicyId = policyId},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            return Task.FromResult(ResponseCodes.OK(issuance));
        }
    }

    public class ConfigurationControllerLogic : IConfigurationController
    {
        public Task<Response<ICollection<ConfigurationWithStatus>>> ConfigurationsGetAsync(string policyId)
        {
            var configuration = new ConfigurationWithStatus()
            {
                Configuration = new Configuration() {PolicyId = policyId, Amount = 100, Duration = 100,RequestId = Guid.NewGuid().ToString()},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            ICollection<ConfigurationWithStatus> configurations = new[] {configuration};
            return Task.FromResult(ResponseCodes.OK(configurations));
        }

        public Task<Response<RequestStatus>> ConfigurationsPostAsync(Configuration body, string policyId)
        {
            var status = new RequestStatus {Id = "", Status = RequestState.Completed};
            //TODO: return configuration request id
            return Task.FromResult(ResponseCodes.OK(status));
        }

        public Task<Response<ConfigurationWithStatus>> ConfigurationsGetAsync(string policyId, string configurationId)
        {
            var configuration = new ConfigurationWithStatus()
            {
                Configuration = new Configuration() {PolicyId = policyId, Amount = 100, Duration = 100,RequestId = Guid.NewGuid().ToString()},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            return Task.FromResult(ResponseCodes.OK(configuration));
        }
    }

    public class BusinessTimeControllerLogic : IBusinessTimeController
    {
        public Task<Response<DateTimeOffset>> BusinesstimeGetAsync(string policyId)
        {
            return Task.FromResult(ResponseCodes.OK(DateTimeOffset.Now));
        }

        public Task<Response> BusinesstimePostAsync(DateTimeOffset? body, string policyId)
        {
            //TODO: return business time set request id
            return Task.FromResult(ResponseCodes.OK());
        }
    }

    public class ClaimsControllerLogic : IClaimsController
    {
        public Task<Response<ICollection<ClaimWithStatus>>> ClaimGetAsync(string policyId)
        {
            var claimWithStatus = new ClaimWithStatus()
            {
                Claim = new Claim() {Id = Guid.NewGuid().ToString(), Amount = 10},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            ICollection<ClaimWithStatus> claimWithStatuses = new[] {claimWithStatus};
            return Task.FromResult(ResponseCodes.OK(claimWithStatuses));
        }

        public Task<Response> ClaimPostAsync(Claim body, string policyId)
        {
            //TODO: return claim request id
            return Task.FromResult(ResponseCodes.OK());
        }

        public Task<Response<ClaimWithStatus>> ClaimGetAsync(string policyId, string claimId)
        {
            var claimWithStatus = new ClaimWithStatus()
            {
                Claim = new Claim() {Id = Guid.NewGuid().ToString(), Amount = 10},
                Status = new RequestStatus {Id = Guid.NewGuid().ToString(), Status = RequestState.Completed}
            };
            return Task.FromResult(ResponseCodes.OK(claimWithStatus));
        }
    }
}