using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PolicyExample.Abstractions;

namespace PolicyExample.Domain
{
    public static class AggregateEventsExtensions
    {
        public static Task<IReadOnlyCollection<IAggregateEvent>> ToResult(this IAggregateEvent e)
        {
            return Task.FromResult<IReadOnlyCollection<IAggregateEvent>>(new[] {e});
        }
        public static Task<IReadOnlyCollection<IAggregateEvent>> ToResult(this IAggregateEvent[] e)
        {
            return Task.FromResult<IReadOnlyCollection<IAggregateEvent>>(e);
        }
    }
    public class InsurancePolicy:IAggregate, ISupportSnapshots
    {
        private IEnumerable<IAggregateEvent> ExecuteInner(ICommand command)
        {
            switch (command)
            {
                case IssuePolicyCommand c: 
                    if (_state.Duration ==null) throw new PolicyMissingDurationException();
                    if(_state.Amount <= 0) throw new PolicyZeroAmountException();
                    yield return Apply(new PolicyIssuedEvent(Address.Id, c.IssueDate));
                break;
                    
                case ProcessClaimCommand c:
                    if(_state.IsExpired) throw new PolicyExpiredException();
                    if(!_state.Issued) throw new PolicyNotIssuedException();
                    if(_state.Amount < c.Claim.Amount) throw new PolicyAmountExceedException();
                    yield return Apply(new ClaimFulfilledEvent(Address, c.Claim.Amount)); 
                break;

                case ProcessNewTimeCommand c:
                    yield return Apply(new PolicyTimePassedEvent(Address.Id,c.NewNowTime));
                    if (_state.BusinessTime >= _state.ExpiryDate)
                        yield return Apply(new PolicyExpiredEvent(Address.Id));
                    break;
                
                case SetPolicyAmountCommand c:
                    yield return Apply(new PolicyAmountSetEvent(Address.Id,c.Amount));
                    break;
                
                case SetPolicyDurationCommand c:
                    yield return Apply(new PolicyDurationSetEvent(Address.Id,c.Duration));
                    break;
                default:
                    throw new UnsupportedCommandException();
            }
        }
        public Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command)
        {
            if(command ==null)
                throw new UnsupportedCommandException();
            if(!command.Destination.Equals(Address))
                throw new AggregateIdMismatchException();
            
            return ExecuteInner(command).ToArray().ToResult();
        }
        
        public long Version { get; private set; }

        private PolicyState _state = new PolicyState();
   
         private IAggregateEvent Apply<T>(T @event)where T:IAggregateEvent
         {
             ((IAggregate) this).Apply(@event);
              return @event;
         }
        void IAggregate.Apply(IAggregateEvent @event)
        {
            switch (@event)
            {
                case PolicyCreatedEvent e: Address = e.Source;
                    break;
                case PolicyExpiredEvent e: _state.IsExpired = true;
                    _state.ExpiryDate = e.Occured;
                    break;
                case PolicyIssuedEvent e: _state.Issued = true;
                    _state.IssueDate = e.Issued;
                    _state.ExpiryDate = _state.IssueDate + _state.Duration;
                    break;
                case PolicyAmountSetEvent e: _state.Amount = e.Amount;
                    break;
                case PolicyDurationSetEvent e: _state.Duration = e.Duration;
                    break;
                case PolicyTimePassedEvent e: _state.BusinessTime = e.CurrentTime;
                    break;
                default:
                    Version--;
                    break;
            }
            Version++;
        }

        public AggregateAddress<InsurancePolicy> Address { get; private set; }
        IAggregateAddress IAggregate.Address => Address;
        
        public IAggregate RestoreFromSnapshot(ISnapshot snapshot)
        {
            if (snapshot is SnapshotState<PolicyState> policySnapshot)
            {
                _state = policySnapshot.State;
                Version = policySnapshot.Version;
                Address = new AggregateAddress<InsurancePolicy>(policySnapshot.Address.Id);
            }
            else
                throw new UnsupportedSnapshotException();
            
            return this;
        }

        public ISnapshot BuildSnapshot()
        {
            return new SnapshotState<PolicyState>
            {
                Address = this.Address,
                Version = this.Version,
                Id = Guid.NewGuid().ToString(),
                State = (PolicyState)this._state.Clone(),
            };
        }
    }

    public class UnsupportedSnapshotException : Exception
    {
    }
}