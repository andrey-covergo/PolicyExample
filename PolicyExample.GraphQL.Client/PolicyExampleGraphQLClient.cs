using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using PolicyExample.GraphQL.Types.DTO.Commands;

namespace PolicyExample.GraphQL.Client
{


    public class PolicyExampleGraphQLClient
    {
        public readonly GraphQLHttpClient Client;

        public PolicyExampleGraphQLClient(HttpClient httpClient, string? endpoint=null)
        {
            var options = new GraphQLHttpClientOptions {EndPoint = new Uri(endpoint ?? "https://localhost:5001/graphql")};
            Client = new GraphQLHttpClient(options,
                                            new NewtonsoftJsonSerializer(),
                                            httpClient);
        }
        public async Task<CreateLogicGraphResult> Execute(CreateLogicGraphCommand command)
        {
            var logicGraphCreationRequest = new GraphQLRequest {
                Query = @"
mutation CreateNewGraph($command: CreateLogicGraphCommand) {
  createNewLogicGraph(command: $command){
    success
    errors
    ... on CreateLogicGraphResult{
          logicGraphId
    }
  }
}",
                Variables = new
                {
                    command
                }
            };
            
            var res = await Client.SendMutationAsync<CreateNewGraphRootObject>(logicGraphCreationRequest);
            ValidateResponse(res);
            return res.Data.createNewLogicGraph;
        }

        private static void ValidateResponse<T>(GraphQLResponse<T> res)
        {
            if (res.Errors?.Any() == true)
            {
                throw new GraphQlException(res.Errors);
            }

            if (res.Data == null)
            {
                throw new GraphQlException("received null Data response");
            }
        }

        public async Task<CreateLogicNodeResult> Execute(CreateLogicNodeCommand command)
        {
            var logicGraphCreationRequest = new GraphQLRequest
            {
                Query = @"
mutation CreateNewNode($command: CreateLogicNodeCommand) 
{
  createNewLogicNode(command: $command) 
       {
                errors
                success
                    ... on CreateLogicNodeResult
                    {
                        logicNodeId
                    }
       }
}",
                Variables = new {command}
            };
            
            var res = await Client.SendMutationAsync<CreateNewNodeRootObject>(logicGraphCreationRequest);
            ValidateResponse(res);
            return res.Data.createNewLogicNode;
        } 
        
        public async Task<RunLogicGraphResult> Execute(RunLogicGraphCommand command)
        {
            var logicGraphCreationRequest = new GraphQLRequest
            {
                Query = @"
mutation RunLogicGraph($command: RunLogicGraphCommand)
{
  runLogicGraph(command: $command)
  {
                success
                runReport
                {
                  trace{
                    node{
                       name
                    }
                  }
                }
  }
}",
                Variables = new {command}
            };
            
            var res = await Client.SendMutationAsync<RunLogicGraphRootObject>(logicGraphCreationRequest);
            ValidateResponse(res);
            return res.Data.runLogicGraph;
        }

        private class CreateNewGraphRootObject
        {
            public CreateLogicGraphResult createNewLogicGraph { get; set; }
        }

        private class CreateNewNodeRootObject
        {
            public CreateLogicNodeResult createNewLogicNode { get; set; }
        }

        private class RunLogicGraphRootObject
        {
            public RunLogicGraphResult runLogicGraph { get; set; }
        }

    }
}