using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PolicyExample.API.GraphQL;
using PolicyExample.GraphQL.Types.DTO.Commands;
using Xunit;

namespace PolicyExample.Tests.Integration
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
            int a;
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
            if (res.Errors?.Any()==true)
            {
                throw new GraphQLException(res.Errors);
            }
            if (res.Data==null)
            {
                throw new GraphQLException("received null Data response");
            }
            
            return res.Data.createNewLogicGraph;
        }
        
        class CreateNewGraphRootObject
        {
            public CreateLogicGraphResult createNewLogicGraph { get; set; }
        }

        class CreateNewNodeRootObject
        {
            public CreateLogicNodeResult createNewLogicNode { get; set; }
        }
    
        class RunLogicGraphRootObject
        {
            public RunLogicGraphResult runLogicGraph { get; set; }
        }

    }

    public class GraphQLException : Exception
    {
        public GraphQLError[]? Errors { get; }

        public GraphQLException(GraphQLError[]? errors)
        {
            Errors = errors;
        }
        public GraphQLException(string message):base(message)
        {
        }
    }


    public class LogicGraphCreationTests
    {
        [Fact]
        public async Task Given_createLogicGraph_command_When_executing_it_Then_receive_new_graph_Id()
        {
            var client = SetupClient();
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
}"
                };

            logicGraphCreationRequest.Variables = new 
                                {
                                     command = new {
                                         id="abc",
                                         name= "new graph",
                                         providedEngines = new []{"a","b","c"}
                                     }
                                };

            var res = await client.SendMutationAsync<CreateNewGraphRootObject>(logicGraphCreationRequest);

            res.Errors.Should().BeNullOrEmpty();
            res.Data.createNewLogicGraph.Success.Should().BeTrue();
            res.Data.createNewLogicGraph.Errors.Should().BeNullOrEmpty();
            res.Data.createNewLogicGraph.LogicGraphId.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task Given_createLogicNode__for_root_command_When_executing_it_Then_receive_new_node_Id()
        {
            var client = SetupClient();

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
                    command = new CreateLogicGraphCommand(){Id="abc",Name = "haha"}
                }
            };
            
            var res = await client.SendMutationAsync<CreateNewGraphRootObject>(logicGraphCreationRequest);
            var graphId = res.Data.createNewLogicGraph.LogicGraphId;

            var logicNodeCreationRequest = new GraphQLRequest
            {
                Query = @"
mutation CreateNewNode {
  createNewLogicNode(command: {
    id: ""2""
    name: ""root node""
    logicGraphId:""" + graphId + @"""
            }){
                errors
                success
                    ... on CreateLogicNodeResult{
                    logicNodeId
                }
            }
        }"
            };

            var nodeResult = await client.SendMutationAsync<CreateNewNodeRootObject>(logicNodeCreationRequest);
            nodeResult.Data.createNewLogicNode.Success.Should().BeTrue();
            nodeResult.Data.createNewLogicNode.LogicNodeId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task RunLogicGraph_Result_Deserialization()
        {
            var json = @"
{
  ""runLogicGraph"": {
            ""success"": true,
            ""runReport"": {
                ""trace"": [
                {
                    ""node"": {
                        ""name"": ""root node""
                    }
                }
                ]
            }
        }
    }
";
            var res = JsonConvert.DeserializeObject<RunLogicGraphRootObject>(json);

            res.runLogicGraph.RunReport.Trace.Should().NotBeNullOrEmpty();
        }
        
        [Fact]
        public void Create_Graph_Command_Deserialization_Result()
        {
            var json = @"
            {
                id: ""abc"",
                name: ""new graph"",
                providedEngines: [""a"",""b"",""c""]
            }
";
            var res = JsonConvert.DeserializeObject<CreateLogicGraphCommand>(json);

            res.Name.Should().Be("new graph");
            res.Id.Should().Be("abc");
            res.ProvidedEngines.Should().Equal(new []{"a","b","c"});
        }
        
        [Fact]
        public async Task Given_graph_When_running_it_Then_receive_trace()
        {
            var testHttpclient = SetupTestHostAndClient();
            var graphQlClient = new PolicyExampleGraphQLClient(testHttpclient);
            var client = graphQlClient.Client;

            var createLogicGraphCommand = new CreateLogicGraphCommand(){Id="abc",Name="new graph", ProvidedContexts = new List<string>{"a","b","c"}};
            
            var res = await graphQlClient.Execute(createLogicGraphCommand);
            var graphId = res.LogicGraphId;

            var logicNodeCreationRequest = new GraphQLRequest
            {
                Query = @"
mutation CreateNewNode {
  createNewLogicNode(command: {
    id: ""2""
    name: ""root node""
    logicGraphId:""" + graphId + @"""
            }){
                errors
                success
                    ... on CreateLogicNodeResult{
                    logicNodeId
                }
            }
        }"
            };

            var nodeResult = await client.SendMutationAsync<CreateNewNodeRootObject>(logicNodeCreationRequest);
            
            var runLogicGraphRequest = new GraphQLRequest
            {
                Query = @"
mutation RunLogicGraph {
  runLogicGraph(command: {
    id: ""3""
    logicGraphId:""" + graphId + @"""
            }){
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
        }"
            };
            var trace = await client.SendMutationAsync<RunLogicGraphRootObject>(runLogicGraphRequest);

            trace.Data.runLogicGraph.RunReport.Trace.Should().NotBeNullOrEmpty();
        }
        
        private static GraphQLHttpClient SetupClient()
        {
            var testHttpClient = SetupTestHostAndClient();

            var options = new GraphQLHttpClientOptions {EndPoint = new Uri("https://localhost:5001/graphql")};
            var client = new GraphQLHttpClient(options,
                new NewtonsoftJsonSerializer(),
                testHttpClient);
            return client;
        }

        private static HttpClient SetupTestHostAndClient()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webHostBuilder);
            var testHttpClient = server.CreateClient();
            return testHttpClient;
        }
    }

    class CreateNewGraphRootObject
    {
        public CreateLogicGraphResult createNewLogicGraph { get; set; }
    }

    class CreateNewNodeRootObject
    {
        public CreateLogicNodeResult createNewLogicNode { get; set; }
    }
    
    class RunLogicGraphRootObject
    {
        public RunLogicGraphResult runLogicGraph { get; set; }
    }

}