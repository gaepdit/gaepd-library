using FluentAssertions;
using GaEpd.AppLibrary.GuardClauses;

namespace GaEpd.AppLibrary.Tests.GuardTests;

public class NotNegative
{
    [Test]
    public void DoesNotThrowGivenPositiveValue()
    {
        const int validValue = 1;
        Action act = () => Guard.NotNegative(validValue);
        act.Should().NotThrow();
    }

    [Test]
    public void DoesNotThrowGivenZeroValue()
    {
        const int validValue = 0;
        Action act = () => Guard.NotNegative(validValue);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenValidValue()
    {
        const int validValue = 1;
        var result = Guard.NotNegative(validValue);
        result.Should().Be(validValue);
    }

    [Test]
    public void ThrowsGivenNegativeValue()
    {
        const int negativeValue = -1;
        Action act = () => Guard.NotNegative(negativeValue);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value cannot be negative. (Parameter '{nameof(negativeValue)}')");
    }
}
