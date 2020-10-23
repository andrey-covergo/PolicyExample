using System;
using FluentAssertions;
using PolicyExample.Abstractions;
using PolicyExample.Domain;
using Xunit;

namespace PolicyExample.Tests
{
    public static class AggregateExtensions
    {
        public static IAggregate Apply(this IAggregate aggregate, params IAggregateEvent[] events)
        {
            foreach(var e in events)
                aggregate.Apply(e);

            return aggregate;
        }
    }
    public class PolicyTests
    {
        [Fact]
        public void Given_policy_snapshot_When_restoring_Then_it_succeed()
        {
            var faker = new PolicySnapshotFaker();
            var snapshot = faker.Generate();
            var policy = new Policy();
            policy.RestoreFromSnapshot(snapshot);
            //should we check for internal state? 
            //or check by command execution ? 
        }
        
        [Fact]
        public void Given_policy_When_building_a_snapshot_Then_it_has_fields_from_the_aggregate()
        {
           var policy = new Policy();
           var address = Address.New<Policy>();
           IAggregate policyAggregate = policy;
           
           policyAggregate.Apply(new PolicyCreatedEvent(address.Id));
           policyAggregate.Apply(new PolicyDurationSetEvent(address.Id,TimeSpan.FromDays(30)));
           var issuedDate = DateTimeOffset.Now;
           policyAggregate.Apply(new PolicyIssuedEvent(address.Id, issuedDate));

           var snapshot = (PolicySnapshot)(policy as ISupportSnapshots).BuildSnapshot();
            
           snapshot.Address.Should().BeEquivalentTo(address);
           snapshot.Id.Should().NotBeNullOrEmpty();
           snapshot.Version.Should().Be(policy.Version);
           snapshot.Duration.Should().Be(TimeSpan.FromDays(30));
           snapshot.IssueDate.Should().Be(issuedDate);
        }
        
        [Fact]
        public void Given_policy_When_issuing_without_duration_Then_get_an_error()
        {
            var policy = new Policy();
            IAggregate policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());
            
            policy.Invoking(p=>p.Execute(new IssuePolicyCommand(policy.Address)))
                  .Should().Throw<PolicyMissingDurationException>();
        }
        
        [Fact]
        public void Given_policy_with_duration_When_issuing_without_amount_Then_get_an_error()
        {
            var policy = new Policy();
            IAggregate policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());
            policyAggregate.Apply(new PolicyDurationSetEvent(policy.Address.Id,TimeSpan.FromDays(1)));
            
            policy.Invoking(p=>p.Execute(new IssuePolicyCommand(policy.Address)))
                .Should().Throw<PolicyZeroAmountException>();
        }
        
         
        [Fact]
        public void Given_policy_with_duration_and_amount_When_issuing_Then_get_issued_event()
        {
            var policy = new Policy();
            IAggregate policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());
            policyAggregate.Apply(new PolicyDurationSetEvent(policy.Address.Id,TimeSpan.FromDays(1)));
            policyAggregate.Apply(new PolicyAmountSetEvent(policy.Address.Id,10m));

            var issueDate = DateTimeOffset.Now;
            var events = policy.Execute(new IssuePolicyCommand(policy.Address,issueDate));
            events.Should().BeEquivalentTo(new PolicyIssuedEvent(policy.Address.Id,issueDate));
        }
        
        [Fact]
        public void Given_not_issued_policy_with_duration_and_amount_When_claiming_within_budget_Then_get_error()
        {
            var policy = new Policy();
            IAggregate policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent(),
                                  new PolicyDurationSetEvent(policy.Address.Id,TimeSpan.FromDays(1)),
                                  new PolicyAmountSetEvent(policy.Address.Id,10m));

            var issueDate = DateTimeOffset.Now;
            var events = policy.Execute(new IssuePolicyCommand(policy.Address,issueDate));
            events.Should().BeEquivalentTo(new PolicyIssuedEvent(policy.Address.Id,issueDate));
        }

        [Fact]
        public void Given_not_issued_policy_When_claiming_over_budget_Then_get_error()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                         new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                         new PolicyAmountSetEvent(id,10m));
            
            policy.Invoking(p=>p.Execute(new ProcessClaimCommand(policy.Address, new Claim(1m))))
                  .Should().Throw<PolicyNotIssuedException>();
        }
        
        [Fact]
        public void Given_issued_policy_When_claiming_over_budget_Then_get_error()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id),
                         new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                         new PolicyAmountSetEvent(id,10m),
                         new PolicyIssuedEvent(id, DateTimeOffset.Now));
            
            policy.Invoking(p=>p.Execute(new ProcessClaimCommand(policy.Address, new Claim(100m))))
                 .Should().Throw<PolicyAmountExceedException>();
        }
        
        [Fact]
        public void Given_issued_policy_When_claiming_in_budget_Then_get_payment()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id,10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now));
            
            var events = policy.Execute(new ProcessClaimCommand(policy.Address, new Claim(5m)));
            events.Should().BeEquivalentTo(new ClaimFulfilledEvent(id, 5));
        }
        
        [Fact]
        public void Given_issued_policy_When_time_pass_less_then_duration_Then_policy_does_not_expire()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id,10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now)
            );

            var newTime = DateTimeOffset.Now.AddDays(1000);
            var events = policy.Execute(new ProcessNewTimeCommand(id, newTime));
            events.Should().BeEquivalentTo(new IAggregateEvent[] {new PolicyTimePassedEvent(id, newTime)});
        }
        
        [Fact]
        public void Given_issued_policy_When_time_pass_more_then_duration_Then_policy_expires()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id),
                         new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                         new PolicyAmountSetEvent(id,10m),
                         new PolicyIssuedEvent(id, DateTimeOffset.Now)
                        );


            var newTime = DateTimeOffset.Now.AddDays(1000);
            var events = policy.Execute(new ProcessNewTimeCommand(id, newTime));
            events.Should().BeEquivalentTo( new IAggregateEvent[]{new PolicyTimePassedEvent(id, newTime),
                                                                            new PolicyExpiredEvent(id)});
        }
        
        [Fact]
        public void Given_expired_policy_When_claiming_within_budget_Then_getting_error()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id),
                         new PolicyDurationSetEvent(id,TimeSpan.FromDays(1)),
                         new PolicyAmountSetEvent(id,10m),
                         new PolicyIssuedEvent(id, DateTimeOffset.Now),
                         new PolicyTimePassedEvent(id,  DateTimeOffset.Now.AddDays(1)),
                         new PolicyExpiredEvent(id)
                    );

            policy.Invoking(p=>p.Execute(new ProcessClaimCommand(policy.Address, new Claim(1m))))
                .Should().Throw<PolicyExpiredException>();
        }
        
        [Fact]
        public void Given_a_policy_When_executing_unknown_command_Then_getting_error()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id));

            policy.Invoking(p=>p.Execute(new SomeCommand()))
                  .Should().Throw<PolicyExpiredException>();
        }
        
        [Fact]
        public void Given_a_policy_When_executing_command_for_another_instance_Then_getting_error()
        {
            var policy = new Policy();
            var id = Guid.NewGuid().ToString();
            
            policy.Apply(new PolicyCreatedEvent(id));

            policy.Invoking(p=>p.Execute( new ProcessNewTimeCommand(Guid.NewGuid().ToString(), DateTimeOffset.Now)))
                .Should().Throw<AggregateIdMismatchException>();
        }

        class SomeCommand : ICommand
        {
            public IAggregateAddress Destination { get; }
            public string Id { get; }
        }
    }

}