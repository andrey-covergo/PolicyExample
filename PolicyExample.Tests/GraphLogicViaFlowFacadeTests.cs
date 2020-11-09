using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting.GraphLogic;
using Xunit;

namespace PolicyExample.Tests
{
    public class GraphLogicViaFlowFacadeTests
    {

        class TestFacadeNode : LogicNodeWithFacade
        {
            public Action<NodeBehaviorFacade> Behavior { get; set; } 
            protected override void Execute(IExecutionFlow flow, NodeBehaviorFacade facade)
            {
                if(Behavior!=null)
                    Behavior.Invoke(facade);
            }
        }
        
        [Fact]
        public async Task Given_graph_with_nodes_exposing_flow_facade_When_execute_Then_will_follow_facade_stop_command()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new TestFacadeNode()
            {
                Name = "nodeA", Parent = root,
                Behavior = f => f.StopFlow()
            };
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

            trace.Select(v => v.Node.Name).Should().BeEquivalentTo("root","nodeA");
        }
        
        
        [Fact]
        public async Task Given_graph_with_nodes_exposing_flow_facade_When_redirect_to_not_ordered_child_via_facade_Then_will_follow_facade_redirection()
        {
            var root = new LogicNode() {Name = "root"};
            var childA = new TestFacadeNode()
            {
                Name = "nodeA", Parent = root,
                Behavior = f => f.RedirectFlowToChild(1) //redirect to nodeAB instead of nodeAA
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
                Root = root, ExecutionFlow = new OrderedExecutionFlow()
            };

            var trace = new List<NodeVisitResult>();
            await foreach (var visit in graph.Run())
            {
                trace.Add(visit);
            }

            trace.Select(v => v.Node.Name)
                .Should().BeEquivalentTo("root","nodeA", "nodeAB", "nodeAA","nodeB","nodeBA","nodeBB");
        }
    }
}