using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using PolicyExample.Domain;
using PolicyExample.Domain.Command;
using PolicyExample.Domain.Events;
using PolicyExample.Scripting;
using Xunit;

namespace PolicyExample.Tests
{
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
}