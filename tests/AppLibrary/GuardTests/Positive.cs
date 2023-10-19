using GaEpd.AppLibrary.GuardClauses;

namespace GaEpd.AppLibrary.Tests.GuardTests;

public class Positive
{
    [Test]
    public void DoesNotThrowGivenPositiveValue()
    {
        const int validValue = 1;
        Action act = () => Guard.Positive(validValue);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenValidValue()
    {
        const int validValue = 1;
        var result = Guard.Positive(validValue);
        result.Should().Be(validValue);
    }

    [Test]
    public void ThrowsGivenNegativeValue()
    {
        const int negativeValue = -1;
        Action act = () => Guard.Positive(negativeValue);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value must be positive (greater than zero). (Parameter '{nameof(negativeValue)}')");
    }

    [Test]
    public void ThrowsGivenZeroValue()
    {
        const int zeroValue = 0;
        Action act = () => Guard.Positive(zeroValue);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value must be positive (greater than zero). (Parameter '{nameof(zeroValue)}')");
    }
}
