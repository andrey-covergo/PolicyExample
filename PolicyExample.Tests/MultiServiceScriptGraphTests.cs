using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting;
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
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new JintNodeExecutor())
            };

            var result = await graph.Run().ToListAsync();

            result.Should().HaveCount(1);
            var error = result.First().Result.Should().BeOfType<ExecutionError>().Subject;
            error.Message.Should().Contain(nameof(MissingServiceException));
        }
        
        [Fact]
        public void Given_graph_with_node_with_missing_services_When_validating_graph_Then_error_occurs()
        {
            var root = new LogicNode()
            {
                Name = "root",
                Script = new Script("bad script",
                    Language.JavaScriptEs5,
                    KnownServices.ExecutionFlowService)

            };

            var executor =   new JintNodeExecutor();
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow(executor)
            };

            
            
            graph.Invoking(g => executor.ValidateAll(graph.Root))
                 .Should()
                 .Throw<MissingServiceException>();
        }
    }
}