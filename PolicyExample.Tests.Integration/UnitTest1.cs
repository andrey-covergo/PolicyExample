using System;
using System.Threading.Tasks;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using PolicyExample.API.GraphQL;
using PolicyExample.GraphQL.Types.DTO.Commands;
using Xunit;

namespace PolicyExample.Tests.Integration
{
    public class LogicGraphCreationTests
    {
        [Fact]
        public async Task Given_createLogicGraph_command_When_executing_it_Then_receive_new_graph_Id()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            using var server = new TestServer(webHostBuilder);
            var testHttpClient = server.CreateClient();

            var options = new GraphQLHttpClientOptions {EndPoint = new Uri("https://localhost:5001/graphql")};
            var client = new GraphQLHttpClient(options, 
                                new NewtonsoftJsonSerializer(),
                                testHttpClient);
            
                
            var logicGraphCreationRequest = new GraphQLRequest {
                    Query = @"
mutation CreateNewGraph {
  createNewLogicGraph(command: {
    id: ""abc""
    name: ""new graph""
    providedEngines: [""a"",""b"",""c""]
}){
    success
    errors
    ... on CreateLogicGraphResult{
          logicGraphId
    }
  }
}"
                };

            var res = await client.SendMutationAsync<RootObject<CreateLogicGraphResult>>(logicGraphCreationRequest);

            res.Errors.Should().BeNullOrEmpty();
            res.Data.createNewLogicGraph.Success.Should().BeTrue();
            res.Data.createNewLogicGraph.Errors.Should().BeNullOrEmpty();
            res.Data.createNewLogicGraph.LogicGraphId.Should().NotBeEmpty();
        }
    }

    class RootObject<T>
    {
        public T createNewLogicGraph { get; set; }
    }
}