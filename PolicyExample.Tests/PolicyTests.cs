using System;
using System.Threading.Tasks;
using FluentAssertions;
using PolicyExample.Abstractions;
using PolicyExample.Domain;
using PolicyExample.Domain.Command;
using PolicyExample.Domain.Events;
using PolicyExample.Domain.Exceptions;
using Xunit;

namespace PolicyExample.Tests
{
   



    public class PolicyTests
    {
        [Fact]
        public void Given_policy_snapshot_When_restoring_Then_it_succeed()
        {
            var snapshot = new PolicyState
            {
                Amount = 5,
                BusinessTime = DateTimeOffset.Now,
                Duration = TimeSpan.FromDays(1),
                ExpiryDate = DateTimeOffset.Now.AddDays(10),
                IsExpired = false,
                Issued = true,
                IssueDate = DateTimeOffset.Now
            };
            var policy = new InsurancePolicy();
            policy.RestoreFromSnapshot(new SnapshotState<PolicyState>
            {
                State = snapshot, Address = new AggregateAddress<InsurancePolicy>(), Id = Guid.NewGuid().ToString(),
                Version = 100
            });
        }

        [Fact]
        public void Given_setup_policy_When_restoring_from_snapshot_Then_policy_is_the_same_as_before()
        {
            var policy = new InsurancePolicy();
            var address = Address.New<InsurancePolicy>();
            IAggregateRoot policyAggregate = policy;

            policyAggregate.Apply(new PolicyCreatedEvent(address.Id));
            var durationSetEvent =
                policyAggregate.ApplyEvent(new PolicyDurationSetEvent(address.Id, TimeSpan.FromDays(30)));
            var policyAmountEvent = policyAggregate.ApplyEvent(new PolicyAmountSetEvent(address.Id, 20m));
            var issuedEvent = policyAggregate.ApplyEvent(new PolicyIssuedEvent(address.Id, DateTimeOffset.Now));
            policyAggregate.Apply(new PolicyTimePassedEvent(address.Id, DateTimeOffset.Now.AddMinutes(1)));
            policyAggregate.Apply(new ClaimFulfilledEvent(address.Id, 2m));
            policyAggregate.Apply(new PolicyExpiredEvent(address.Id));

            var snapshot = (SnapshotState<PolicyState>) (policy as ISupportSnapshots).BuildSnapshot();

            snapshot.Address.Should().BeEquivalentTo(address);
            snapshot.Id.Should().NotBeNullOrEmpty();
            snapshot.Version.Should().Be(policy.Version);
            snapshot.State.Duration.Should().Be(durationSetEvent.Duration);
            snapshot.State.IssueDate.Should().Be(issuedEvent.Issued);

            var restoredSnapshot =
                ((InsurancePolicy) new InsurancePolicy().RestoreFromSnapshot(snapshot)).BuildSnapshot();
            snapshot.Should().BeEquivalentTo(restoredSnapshot, o => o.Excluding(s => s.Id));
        }


        [Fact]
        public void Given_policy_When_building_a_snapshot_Then_it_has_fields_from_the_aggregate()
        {
            var policy = new InsurancePolicy();
            var address = Address.New<InsurancePolicy>();
            IAggregateRoot policyAggregate = policy;

            policyAggregate.Apply(new PolicyCreatedEvent(address.Id));
            policyAggregate.Apply(new PolicyDurationSetEvent(address.Id, TimeSpan.FromDays(30)));
            var issuedDate = DateTimeOffset.Now;
            policyAggregate.Apply(new PolicyIssuedEvent(address.Id, issuedDate));

            var snapshot = (SnapshotState<PolicyState>) (policy as ISupportSnapshots).BuildSnapshot();

            snapshot.Address.Should().BeEquivalentTo(address);
            snapshot.Id.Should().NotBeNullOrEmpty();
            snapshot.Version.Should().Be(policy.Version);
            snapshot.State.Duration.Should().Be(TimeSpan.FromDays(30));
            snapshot.State.IssueDate.Should().Be(issuedDate);
        }

        [Fact]
        public void Given_policy_When_issuing_without_duration_Then_get_an_error()
        {
            var policy = new InsurancePolicy();
            IAggregateRoot policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());

            policy.Invoking(async p => await p.Execute(new IssuePolicyCommand(policy.Address)))
                .Should().Throw<PolicyMissingDurationException>();
        }

        [Fact]
        public void Given_policy_When_apply_event_Then_version_is_changed()
        {
            var policy = new InsurancePolicy();
            IAggregateRoot policyAggregate = policy;
            policy.Version.Should().Be(0);
            policyAggregate.Apply(new PolicyCreatedEvent());
        }

        [Fact]
        public void Given_policy_with_duration_When_issuing_without_amount_Then_get_an_error()
        {
            var policy = new InsurancePolicy();
            IAggregateRoot policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());
            policyAggregate.Apply(new PolicyDurationSetEvent(policy.Address.Id, TimeSpan.FromDays(1)));

            policy.Invoking(async p => await p.Execute(new IssuePolicyCommand(policy.Address)))
                .Should().Throw<PolicyZeroAmountException>();
        }


        [Fact]
        public async Task Given_policy_with_duration_and_amount_When_issuing_Then_get_issued_event()
        {
            var policy = new InsurancePolicy();
            IAggregateRoot policyAggregate = policy;
            policyAggregate.Apply(new PolicyCreatedEvent());
            policyAggregate.Apply(new PolicyDurationSetEvent(policy.Address.Id, TimeSpan.FromDays(1)));
            policyAggregate.Apply(new PolicyAmountSetEvent(policy.Address.Id, 10m));

            var issueDate = DateTimeOffset.Now;
            var events = await policy.Execute(new IssuePolicyCommand(policy.Address, issueDate));
            events.Should().BeLike(new PolicyIssuedEvent(policy.Address.Id, issueDate));
        }

        [Fact]
        public void Given_not_issued_policy_with_duration_and_amount_When_claiming_within_budget_Then_get_error()
        {
            var policy = new InsurancePolicy();
            //IAggregate policyAggregate = policy;
            var id = Guid.NewGuid().ToString();
            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m));

            policy.Invoking(async p => await p.Execute(new ProcessClaimCommand(policy.Address, new Claim(1m))))
                .Should().Throw<PolicyNotIssuedException>();
        }

        [Fact]
        public void Given_not_issued_policy_When_claiming_over_budget_Then_get_error()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m));

            policy.Invoking(async p => await p.Execute(new ProcessClaimCommand(policy.Address, new Claim(1m))))
                .Should().Throw<PolicyNotIssuedException>();
        }

        [Fact]
        public void Given_issued_policy_When_claiming_over_budget_Then_get_error()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now));

            policy.Invoking(async p => await p.Execute(new ProcessClaimCommand(policy.Address, new Claim(100m))))
                .Should().Throw<PolicyAmountExceedException>();
        }

        [Fact]
        public async Task Given_issued_policy_When_claiming_in_budget_Then_get_payment()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now));

            var events = await policy.Execute(new ProcessClaimCommand(policy.Address, new Claim(5m)));
            events.Should().BeLike(new ClaimFulfilledEvent(id, 5));
        }

        [Fact]
        public async Task Given_issued_policy_When_time_pass_less_then_duration_Then_policy_does_not_expire()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(10)),
                new PolicyAmountSetEvent(id, 10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now)
            );

            var newTime = DateTimeOffset.Now.AddDays(1);
            var events = await policy.Execute(new ProcessNewTimeCommand(id, newTime));
            events.Should().BeLike(new PolicyTimePassedEvent(id, newTime));
        }

        [Fact]
        public async Task Given_issued_policy_When_time_pass_more_then_duration_Then_policy_expires()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now)
            );


            var newTime = DateTimeOffset.Now.AddDays(1000);
            var events = await policy.Execute(new ProcessNewTimeCommand(id, newTime));
            events.Should().BeLike(new PolicyTimePassedEvent(id, newTime), new PolicyExpiredEvent(id));
        }

        [Fact]
        public void Given_expired_policy_When_claiming_within_budget_Then_getting_error()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id),
                new PolicyDurationSetEvent(id, TimeSpan.FromDays(1)),
                new PolicyAmountSetEvent(id, 10m),
                new PolicyIssuedEvent(id, DateTimeOffset.Now),
                new PolicyTimePassedEvent(id, DateTimeOffset.Now.AddDays(1)),
                new PolicyExpiredEvent(id)
            );

            policy.Invoking(async p => await p.Execute(new ProcessClaimCommand(policy.Address, new Claim(1m))))
                .Should().Throw<PolicyExpiredException>();
        }

        [Fact]
        public void Given_a_policy_When_executing_unknown_command_Then_getting_error()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id));

            policy.Invoking(
                    async p => await p.Execute(new SomeCommand {Destination = Address.New<InsurancePolicy>(id)}))
                .Should().Throw<UnsupportedCommandException>();
        }

        [Fact]
        public void Given_a_policy_When_executing_command_for_another_instance_Then_getting_error()
        {
            var policy = new InsurancePolicy();
            var id = Guid.NewGuid().ToString();

            policy.Apply(new PolicyCreatedEvent(id));

            policy.Invoking(async p =>
                    await p.Execute(new ProcessNewTimeCommand(Guid.NewGuid().ToString(), DateTimeOffset.Now)))
                .Should().Throw<AggregateIdMismatchException>();
        }

        private class SomeCommand : ICommand
        {
            public IAggregateAddress Destination { get; set; }
            public string Id { get; }
        }
    }
}