using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PolicyExample.Abstractions;
using PolicyExample.Domain.Command;
using PolicyExample.Domain.Events;
using PolicyExample.Domain.Exceptions;

namespace PolicyExample.Domain
{
    public class InsurancePolicy : IAggregateRoot, ISupportSnapshots
    {
        private PolicyState _state = new PolicyState();

        public AggregateAddress<InsurancePolicy> Address { get; private set; }

        public Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command)
        {
            if (command == null)
                throw new UnsupportedCommandException();
            if (!command.Destination.Equals(Address))
                throw new AggregateIdMismatchException();

            return ExecuteInner(command).ToArray().ToResult();
        }

        public long Version { get; private set; }

        void IAggregateRoot.Apply(IAggregateEvent @event)
        {
            switch (@event)
            {
                case PolicyCreatedEvent e:
                    Address = e.Source;
                    break;
                case PolicyExpiredEvent e:
                    _state.IsExpired = true;
                    _state.ExpiryDate = e.Occured;
                    break;
                case PolicyIssuedEvent e:
                    _state.Issued = true;
                    _state.IssueDate = e.Issued;
                    _state.ExpiryDate = _state.IssueDate + _state.Duration;
                    break;
                case PolicyAmountSetEvent e:
                    _state.Amount = e.Amount;
                    break;
                case PolicyDurationSetEvent e:
                    _state.Duration = e.Duration;
                    break;
                case PolicyTimePassedEvent e:
                    _state.BusinessTime = e.CurrentTime;
                    break;
                default:
                    Version--;
                    break;
            }

            Version++;
        }

        IAggregateAddress IAggregateRoot.Address => Address;

        public IAggregateRoot RestoreFromSnapshot(ISnapshot snapshot)
        {
            if (snapshot is SnapshotState<PolicyState> policySnapshot)
            {
                _state = policySnapshot.State;
                Version = policySnapshot.Version;
                Address = new AggregateAddress<InsurancePolicy>(policySnapshot.Address.Id);
            }
            else
            {
                throw new UnsupportedSnapshotException();
            }

            return this;
        }

        public ISnapshot BuildSnapshot()
        {
            return new SnapshotState<PolicyState>
            {
                Address = Address,
                Version = Version,
                Id = Guid.NewGuid().ToString(),
                State = (PolicyState) _state.Clone()
            };
        }

        private IEnumerable<IAggregateEvent> ExecuteInner(ICommand command)
        {
            switch (command)
            {
                case IssuePolicyCommand c:
                    if (_state.Duration == null) throw new PolicyMissingDurationException();
                    if (_state.Amount <= 0) throw new PolicyZeroAmountException();
                    yield return Apply(new PolicyIssuedEvent(Address.Id, c.IssueDate));
                    break;

                case ProcessClaimCommand c:
                    if (_state.IsExpired) throw new PolicyExpiredException();
                    if (!_state.Issued) throw new PolicyNotIssuedException();
                    if (_state.Amount < c.Claim.Amount) throw new PolicyAmountExceedException();
                    yield return Apply(new ClaimFulfilledEvent(Address, c.Claim.Amount));
                    break;

                case ProcessNewTimeCommand c:
                    yield return Apply(new PolicyTimePassedEvent(Address.Id, c.NewNowTime));
                    if (_state.BusinessTime >= _state.ExpiryDate)
                        yield return Apply(new PolicyExpiredEvent(Address.Id));
                    break;

                case SetPolicyAmountCommand c:
                    yield return Apply(new PolicyAmountSetEvent(Address.Id, c.Amount));
                    break;

                case SetPolicyDurationCommand c:
                    yield return Apply(new PolicyDurationSetEvent(Address.Id, c.Duration));
                    break;
                default:
                    throw new UnsupportedCommandException();
            }
        }

        private IAggregateEvent Apply<T>(T @event) where T : IAggregateEvent
        {
            ((IAggregateRoot) this).Apply(@event);
            return @event;
        }
    }
}