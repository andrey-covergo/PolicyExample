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
        public void Given_graph_with_nodes_When_execute_Then_will_travers_nodes_in_right_direction()
        {


            var root = new LogicNode() {Name = "root"};
            var child = new LogicNode() {Name = "child", Parent = root};
            root.Children.Add(child);
            
            var graph = new LogicGraph(){
                Root = root,
                
            
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