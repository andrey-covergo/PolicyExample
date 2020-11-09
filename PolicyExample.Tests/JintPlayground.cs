using FluentAssertions;
using Jint;
using Jint.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace PolicyExample.Tests
{
    public class JintPlayground
    {
        private readonly ITestOutputHelper _output;

        public JintPlayground(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void LogFromJint()
        {
            var engine = new Engine()
                    .SetValue("log", _output)
                ;
    
            engine.Execute(@"
                  function hello() { 
                    log.writeLine('Hello World');
                  };
                  
                  hello();
                ");
        }

        class TestCalculator
        {
            public int Add(int a, int b) => a + b;
            internal int InternalAdd(int a, int b) => Add(a,b);
        }
        [Fact]
        public void Execute_Method_from_external_object_and_return_value()
        {
            var calculator = new TestCalculator();
            
            var engine = new Engine().SetValue("calculator", calculator);
    
            engine.Execute(@"
                  function calc() { 
                     return calculator.Add(1,2);
                  };
                  var result = calc();
                ");
            var value = engine.GetValue("result");
            value.ToObject().As<double>().Should().Be(calculator.Add(1,2));
        }  
        
        [Fact]
        public void Execute_Method_from_external_object_and_return_value_without_function()
        {
            var calculator = new TestCalculator();
            
            var engine = new Engine().SetValue("calculator", calculator);
    
            engine.Execute(@"
                 calculator.Add(1,2);
                ");
            var value = engine.GetCompletionValue();
            value.ToObject().As<double>().Should().Be(calculator.Add(1,2));
        }
        
        [Fact]
        public void Cannot_Execute_internal_Method_from_external_object()
        {
            var calculator = new TestCalculator();
            
            var engine = new Engine().SetValue("calculator", calculator);
    
            engine.Invoking( e => e.Execute(@"
                 calculator.InternalAdd(1,2);
                ")).Should().Throw<JavaScriptException>().WithMessage("Object has no method 'InternalAdd'");
           
        }
    }
}