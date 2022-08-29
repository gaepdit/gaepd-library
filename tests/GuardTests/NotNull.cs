using FluentAssertions;
using GaEpd.Library.GuardClauses;

namespace GaEpd.Library.Tests.GuardTests;

public class NotNullOrWhiteSpace
{
    [Test]
    public void DoesNotThrowGivenNonNullValue()
    {
        var notNullObject = new object();
        Action act = () => Guard.NotNull(notNullObject);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenNonNullValue()
    {
        const string validValue = "a";
        var result = Guard.NotNull(validValue);
        result.Should().Be(validValue);
    }

    [Test]
    public void ThrowsGivenNullValue()
    {
        object? nullObject = null;
        Action act = () => Guard.NotNull(nullObject);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{nameof(nullObject)}')");
    }
}
