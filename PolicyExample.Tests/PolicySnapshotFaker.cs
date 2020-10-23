using Bogus;
using PolicyExample.Domain;

namespace PolicyExample.Tests
{
    public class PolicySnapshotFaker : Faker<PolicySnapshot> {
        public PolicySnapshotFaker() {
            RuleFor(o => o.Version, f => f.Random.Number(1, 1000));
            RuleFor(o => o.Duration, f => f.Date.Timespan());
            RuleFor(o => o.Id, f => f.Lorem.Slug());
        }
    }
}