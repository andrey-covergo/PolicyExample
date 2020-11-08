using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Collections;
using PolicyExample.Abstractions;
using PolicyExample.Domain;
using PolicyExample.Domain.Command;
using PolicyExample.Domain.Events;
using Xunit;

namespace PolicyExample.Tests
{

    
    /// <summary>
    /// Provides a new context to a script
    /// By the end of script execution Apply() must be called to accept any changes
    /// made in the context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISupportScripting<T>
    {
        IScriptContext<T> CreateContext { get; }
        Task Apply(IScriptContext<T> context);

    }
    public interface IRunResult : ISerializable
    {
        string Id { get; }
        ISerializable Result { get; }
    }

    /// <summary>
    /// T expects a script will call any public method or property T exposes
    /// </summary> 
    /// <typeparam name="T"></typeparam>
    public interface IScriptContext<T>
    {
        public string EngineVersion { get;  }
        public string ContextVersion { get; }
        public T Context { get; }
        
    }
    public interface IScriptEngine
    {
        public Task<IRunResult> Run<T>(IScript script,IScriptContext<T> context); //return result 
    }

    public interface IScript
    {
        Task<IRunResult> Run<T>(IScriptContext<T> context);
    }

    public interface ILogicNode
    {
        ILogicNode Parent { get; }
        ILogicNode[] Children { get; }
        string Name { get; }
        string Id { get; }
        Task<NodeExecutionResult> Execute();

    }

    public class NodeExecutionResult
    {
        public string CorrelationId { get; }
        public string NodeId { get; }
        public string Message { get; protected set; }
        public ILogicNode[] Trace {get;}
    }
    
    public class ExecutionSuccessAndContinue : NodeExecutionResult
    {
        public ILogicNode NextNode { get; }
    }
    public class ExecutionSuccessAndStop : NodeExecutionResult
    {
        public ISerializable Result { get; }
    }
    
    public class ExecutionError : NodeExecutionResult
    {
        public string Message { get; }
    }


    public class PolicyScriptingTests
    {
        [Fact]
        public async Task Given_scriptedPolicy_When_execute_cmd_Then_script_is_involved()
        {
            // Policy defines the extensions point 
            // Scripting engine provides basic abstractions: 
            // set script command 
            // script-related event 
            // Scripting engine definition
            // script sanitizers
            // read model build for pure calculations (without an aggregate state change)
            
            var policy =  new InsurancePolicy();
            var policyCreatedEvent = new PolicyCreatedEvent("test_policy");
            policy.Apply(policyCreatedEvent);
            policy.Apply(new IssuePolicyScriptSetEvent("test_policy", ""));

            var result = await policy.Execute(new IssuePolicyCommand(policyCreatedEvent.Source));
            
            
            throw new NotImplementedException();
        }
    }

    public class IssuePolicyScriptSetEvent : PolicyEvent
    {
        public IssuePolicyScriptSetEvent(string source, string script) : base(source)
        {
            throw new NotImplementedException();
        }
    }

    public static class AggregateEventsAssertionExtensions
    {
        public static AndConstraint<GenericCollectionAssertions<IAggregateEvent>> BeLike(
            this GenericCollectionAssertions<IAggregateEvent> assert,
            params IAggregateEvent[] expected)
        {
            return assert.BeEquivalentTo(expected,
                ob =>
                {
                    return ob.Excluding(e => e.Id)
                        .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                        .When(info => info.SelectedMemberPath.EndsWith(nameof(IAggregateEvent.Occured)));
                });
        }
    }
}