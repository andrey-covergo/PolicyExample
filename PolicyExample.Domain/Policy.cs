using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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
    public class Policy:IAggregate, ISupportSnapshots
    {
        public Task<IReadOnlyCollection<IAggregateEvent>> Execute(ICommand command)
        {
            if(command ==null)
                throw new UnsupportedCommandException();
            if(command.Destination != Address)
                throw new AggregateIdMismatchException();
            
            switch (command)
            {
                case IssuePolicyCommand c: 
                    if (_duration ==null) throw new PolicyMissingDurationException();
                    if(_amount <= 0) throw new PolicyZeroAmountException();
                       return new PolicyIssuedEvent(Address.Id, c.IssueDate).ToResult();
                    
                case ProcessClaimCommand c:
                    if(_isExpired) throw new PolicyExpiredException();
                    if(!_issued) throw new PolicyNotIssuedException();
                    if(_amount < c.Claim.Amount) throw new PolicyAmountExceedException();
                    return new ClaimFulfilledEvent(Address,c.Claim.Amount).ToResult();
                
                case ProcessNewTimeCommand c:
                    return new PolicyTimePassedEvent(Address.Id,c.NewNowTime).ToResult();
                
                case SetPolicyAmountCommand c:
                    return new PolicyAmountSetEvent(Address.Id,c.Amount).ToResult();
                
                case SetPolicyDurationCommand c:
                    return new PolicyDurationSetEvent(Address.Id,c.Duration).ToResult();
                default:
                    throw new UnsupportedCommandException();
            }
        }
        
        public long Version { get; private set; }

        private bool _issued;
        private TimeSpan? _duration;
        private DateTimeOffset? _issuedDate;
        private DateTimeOffset? _expiryDate;
        private bool _isExpired;
        private decimal _amount;
        private DateTimeOffset _businessTime;
        
        void IAggregate.Apply(IAggregateEvent @event)
        {
            switch (@event)
            {
                case PolicyCreatedEvent e: Address = e.Source;
                    break;
                case PolicyExpiredEvent e: _isExpired = true;
                    _expiryDate = e.Occured;
                    break;
                case PolicyIssuedEvent e: _issued = true;
                    _issuedDate = e.Issued;
                    break;
                case PolicyAmountSetEvent e: _amount = e.Amount;
                    break;
                case PolicyDurationSetEvent e: _duration = e.Duration;
                    break;
                case PolicyTimePassedEvent e: _businessTime = e.CurrentTime;
                    break;
            }
        }

        public AggregateAddress<Policy> Address { get; private set; }
        IAggregateAddress IAggregate.Address => Address;

        public IAggregate RestoreFromSnapshot(ISnapshot snapshot)
        {
            if (snapshot is PolicySnapshot policySnapshot)
            {
                Address = policySnapshot.Address;
                Version = policySnapshot.Version;
                _duration = policySnapshot.Duration;
                _issuedDate = policySnapshot.IssueDate;
            }

            return this;
        }

        public ISnapshot BuildSnapshot()
        {
            return new PolicySnapshot
            {
                Address = this.Address,
                Version = this.Version,
                Id = Guid.NewGuid().ToString(),
                Duration = this._duration,
                IssueDate = this._issuedDate
            };
        }
    }
}