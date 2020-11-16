using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting;
using PolicyExample.Scripting.GraphLogic;
using PolicyExample.Scripting.Jint;
using Xunit;

namespace PolicyExample.Tests
{
    public class GraphLogicWithJintTests
    {

        [Fact]
        public async Task Given_graph_with_jint_node_raising_an_error_When_execute_Then_flow_stops()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new LogicNode()
            {
                Name = "nodeA", Parent = root,
                Script = new JintScript("bad script")
            };
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childA);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childA};
            childA.Children.Add(nodeAA);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new JintNodeExecutor())
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().Equal("root","nodeA");

            var error = trace.Last().Result.Should().BeOfType<ExecutionError>().Subject;
            error.Message.Should().Contain("Unexpected identifier");
        }
            
        [Fact]
        public async Task Given_graph_with_jint_node_stopping_flow_When_execute_Then_will_follow_jint_stop_command()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new LogicNode()
            {
                Name = "nodeA", Parent = root,
                Script = new JintScript("flow.Stop();")
            };
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childA);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childA};
            childA.Children.Add(nodeAA);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new JintNodeExecutor())
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().Equal("root","nodeA");
        }
        
        [Fact]
        public async Task Given_graph_with_jint_node_redirecting_to_not_ordered_child_When_execute_Then_will_follow_jint_redirection()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new LogicNode()
            {
                Name = "nodeA", Parent = root,
                Script = new JintScript("flow.RedirectToChild(1);")
            };
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childA);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childA};
            childA.Children.Add(nodeAA);
            
            var nodeAB = new LogicNode() {Name = "nodeAB", Parent = childA};
            childA.Children.Add(nodeAB);
            
            //to check that a normal flow without redirection with pick A child before B child as expected
            var nodeBA = new LogicNode() {Name = "nodeBA", Parent = childB};
            childB.Children.Add(nodeBA);
            
            var nodeBB = new LogicNode() {Name = "nodeBB", Parent = childB};
            childB.Children.Add(nodeBB);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new JintNodeExecutor())
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name)
                .Should().Equal("root","nodeA", "nodeAB", "nodeAA","nodeB","nodeBA","nodeBB");
        }
    }
}