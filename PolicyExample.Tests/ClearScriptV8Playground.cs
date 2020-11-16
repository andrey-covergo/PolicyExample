using FluentAssertions;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Xunit;
using Xunit.Abstractions;

namespace PolicyExample.Tests
{
    public class ClearScriptV8Playground
    {
        private readonly ITestOutputHelper _output;

        public ClearScriptV8Playground(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void LogFromV8()
        {
            using var engine = new V8ScriptEngine();
                
            engine.AddHostObject("output", _output);
    
            engine.Execute(@"
                  function hello() { 
                    output.WriteLine('Hello World');
                  };
                  
                  hello();
                ");
        }

        public class TestCalculator
        {
            public int Add(int a, int b) => a + b;
           
            internal int InternalAdd(int a, int b) => Add(a,b);
        }
        
        [Fact]
        public void Execute_Method_from_external_object_and_return_value()
        {
            var calculator = new TestCalculator();
            using var engine = new V8ScriptEngine();
            engine.AddHostObject("calculator", calculator);
    
            engine.Execute(@"
                  function calc() { 
                     return calculator.Add(1,2);
                  };
                  var result = calc();
                ");
            
            var value = (int)engine.Script.result;
            
           
            value.Should().Be(calculator.Add(1,2));

            _output.WriteLine(value.ToString());
        }  
        
        [Fact]
        public void Execute_Method_from_external_object_and_return_value_without_function()
        {
            var calculator = new TestCalculator();
            using var engine = new V8ScriptEngine();
            engine.AddHostObject("calculator", calculator);
    
            engine.Execute(@"var res = calculator.Add(1,2)");
            int value = engine.Script.res;
            value.Should().Be(calculator.Add(1,2));
        }
        
        [Fact]
        public void Cannot_Execute_internal_Method_from_external_object()
        {
            var calculator = new TestCalculator();
            using var engine = new V8ScriptEngine();
            engine.AddHostObject("calculator", calculator);
    
            engine.Invoking( e => e.Execute(@"
                 calculator.InternalAdd(1,2);
                ")).Should().Throw<ScriptEngineException>().WithMessage("TypeError: calculator.InternalAdd is not a function");
           
        }
    }
}