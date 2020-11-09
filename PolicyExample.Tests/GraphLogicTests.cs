using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting.GraphLogic;
using Xunit;

namespace PolicyExample.Tests
{
    public class GraphLogicTests
    {
        [Fact]
        public async Task Given_graph_with_nodes_When_execute_Then_will_travers_nodes_in_right_direction()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new LogicNode() {Name = "nodeA", Parent = root};
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childA);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childA};
            childA.Children.Add(nodeAA);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().BeEquivalentTo("root","nodeA", "nodeAA", "nodeB");
        }

        class TestLogicNode : LogicNode
        {
            public Func<IExecutionFlow,NodeExecutionResult>? Behavior { get; set; }

            public override Task<NodeExecutionResult> Execute(IExecutionFlow flow)
            {
                return Behavior == null ? base.Execute(flow) : Task.FromResult(Behavior.Invoke(flow));
            }
        }
        [Fact]
        public async Task Given_nodes_changing_direction_When_execute_Then_will_follow_it_commands()
        {
            var root = new LogicNode() {Name = "root"};
            var childARedirectParent = new TestLogicNode() {Name = "nodeA", Parent = root, 
                Behavior = f=> new ExecutionSuccessAndRedirect(root)};
            
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childARedirectParent);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childARedirectParent};
            childARedirectParent.Children.Add(nodeAA);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };

            var trace = new List<NodeVisitResult>();
            
            
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().BeEquivalentTo("root","nodeA", "nodeB");
        }
        
        [Fact]
        public async Task Given_nodes_stopping_flow_When_execute_Then_will_follow_it_commands()
        {
            var root = new LogicNode() {Name = "root"};
            var childAStop = new TestLogicNode() {Name = "nodeA", Parent = root, 
                Behavior = f=> new ExecutionSuccessAndStop(){Result = 1}};
            
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childAStop);
            root.Children.Add(childB);

            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };

            var trace = new List<NodeVisitResult>();
            
            
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().BeEquivalentTo("root","nodeA");

            trace.Last().Result.As<ExecutionSuccessAndStop>().Result.Should().Be(1);
        }
    }
}