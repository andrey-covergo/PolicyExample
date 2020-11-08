using System;
using FluentAssertions;
using FluentAssertions.Collections;
using PolicyExample.Abstractions;

namespace PolicyExample.Tests
{
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