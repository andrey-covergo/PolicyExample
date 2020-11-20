using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting.GraphLogic;
using PolicyExample.Scripting.Jint;
using Xunit;

namespace PolicyExample.Tests
{
    public class MultiServiceScriptGraphTests
    {

        [Fact]
        public async Task Given_graph_with_root_with_missing_services_When_running_Then_error_occurs()
        {

            var root = new LogicNode()
            {
                Name = "root",
                Script = new Script("bad script",
                    Language.JavaScriptEs5,
                    KnownServices.ExecutionFlowService)

            };

            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };

            await graph.Invoking(async g => await AsyncEnumerable.ToListAsync<NodeVisitResult>(g.Run()))
                .Should()
                .ThrowAsync<MissingScriptServicesException>();
        }
        
        [Fact]
        public async Task Given_graph_with_node_with_missing_services_When_validating_graph_Then_error_occurs()
        {

            var root = new LogicNode()
            {
                Name = "root",
                Script = new Script("bad script",
                    Language.JavaScriptEs5,
                    KnownServices.ExecutionFlowService)

            };

            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };


            graph.Invoking(g => g.Validate())
                .Should()
                .Throw<MissingScriptServicesException>();
        }
    }
}