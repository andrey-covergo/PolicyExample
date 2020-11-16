using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting.GraphLogic;
using Xunit;

namespace PolicyExample.Tests
{
    public class GraphLogicFlowServiceTests
    {
        class TestNode : LogicNode
        {
            public Action<NodeFlowService>? Behavior { get; set; }
        }

        class TestFacadeNodeExecutor : INodeExecutor
        {
            public Task<NodeExecutionResult> ExecuteNode(LogicNode node)
            {
                if (node is TestNode facadeNode)
                {
                    var flowService = new NodeFlowService(facadeNode);
                    facadeNode.Behavior?.Invoke(flowService);

                    if(flowService.Result != null)
                        return Task.FromResult(flowService.Result);
                }

                return node.Execute();
            }
        }
        
        [Fact]
        public async Task Given_graph_with_nodes_exposing_flow_facade_When_execute_Then_will_follow_facade_stop_command()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new TestNode()
            {
                Name = "nodeA", Parent = root,
                Behavior = f => f.Stop()
            };
            var childB = new LogicNode() {Name = "nodeB", Parent = root};
            root.Children.Add(childA);
            root.Children.Add(childB);

            var nodeAA = new LogicNode() {Name = "nodeAA", Parent = childA};
            childA.Children.Add(nodeAA);
            
            var graph = new LogicGraph()
            {
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new TestFacadeNodeExecutor())
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name).Should().Equal("root","nodeA");
        }
        
        
        [Fact]
        public async Task Given_graph_with_nodes_exposing_flow_facade_When_redirect_to_not_ordered_child_via_facade_Then_will_follow_facade_redirection()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new TestNode()
            {
                Name = "nodeA", Parent = root,
                Behavior = f => f.RedirectToChild(1) //redirect to nodeAB instead of nodeAA
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
                Root = root, ExecutionFlow = new OrderedExecutionFlow(new TestFacadeNodeExecutor())
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