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
            var client = SetupClient();
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
mutation CreateNewNode {
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

        private static GraphQLHttpClient SetupClient()
        {
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(webHostBuilder);
            var testHttpClient = server.CreateClient();

            var options = new GraphQLHttpClientOptions {EndPoint = new Uri("https://localhost:5001/graphql")};
            var client = new GraphQLHttpClient(options,
                new NewtonsoftJsonSerializer(),
                testHttpClient);
            return client;
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

}