using System;
using System.Threading.Tasks;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using PolicyExample.GraphQL.API;
using PolicyExample.GraphQL.Schema.DTO.Commands;
using PolicyExample.GraphQL.Schema.GraphQLTypes;
using Xunit;

namespace PolicyExample.Tests.Integration
{
    public class LogicGraphCreationTests
    {
        [Fact]
        public async Task Given_createLogicGraph_command_When_executing_it_Then_receive_new_graph_Id()
        {
            //
            // Host.CreateDefaultBuilder(args)
            //     .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            //
            // Build your "app"
            var webHostBuilder = new WebHostBuilder().UseStartup<Startup>();
            using var server = new TestServer(webHostBuilder);
            var testHttpClient = server.CreateClient();
            
            var options = new GraphQLHttpClientOptions();
            options.EndPoint = new Uri("https://localhost:5001/graphql");
            var client = new GraphQLHttpClient(options, 
                                new SystemTextJsonSerializer(),
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
  }
}"
                };

            var res = await client.SendMutationAsync<CreateLogicGraphResult>(logicGraphCreationRequest);

            res.Data.Success.Should().BeTrue();
            res.Data.Errors.Should().BeEmpty();
        }
    }
}