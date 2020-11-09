using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Scripting;
using PolicyExample.Scripting.Abstractions;
using PolicyExample.Scripting.GraphLogic;
using PolicyExample.Scripting.Jint;
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
                Behavior = f=> new ExecutionSuccessAndRedirect(){NextNode = root}};
            
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
    public class ScriptingTests
    {
        class TestCalculator
        {
            public int Add(int a, int b) => a + b;
        }

        class TestContext
        {
            public TestCalculator Calculator { get; set; }
            public int A { get; set; }
            public int B { get; set; }
        }
        [Fact]
        public async Task Given_testScriptEngine_When_executing_script_Then_Script_can_manipulate_context()
        {
            IScriptEngine<IJintScript> engine = new JintScriptEngine();
            var script = new JintScript(@"context.calculator.Add(context.A, context.B)");
            var context = new TestContext(){Calculator = new TestCalculator(), A = 1, B=2};
            var environment = new ScriptEnvironment<TestContext>("1.0","1.0",context);
            var executionRes = await engine.Run(script, environment);

            var result = executionRes.Should().BeOfType<ScriptExecutionSuccess>().Subject;
            result.Result.As<double>().Should().Be(3);
        }
    }
}