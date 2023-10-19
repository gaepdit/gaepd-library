using GaEpd.AppLibrary.GuardClauses;

namespace AppLibrary.Tests.GuardTests;

public class NotNull
{
    [Test]
    public void DoesNotThrowGivenValidValue()
    {
        const string validValue = "a";
        Action act = () => Guard.NotNullOrWhiteSpace(validValue);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenValidValue()
    {
        const string validValue = "a";
        var result = Guard.NotNullOrWhiteSpace(validValue);
        result.Should().Be(validValue);
    }

    [Test]
    public void ThrowsGivenNullValue()
    {
        const string? nullString = null;
        Action act = () => Guard.NotNullOrWhiteSpace(nullString);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{nameof(nullString)}')");
    }

    [Test]
    public void ThrowsGivenWhiteSpaceValue()
    {
        const string? whiteSpaceString = " ";
        Action act = () => Guard.NotNullOrWhiteSpace(whiteSpaceString);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value cannot be null, empty, or white space. (Parameter '{nameof(whiteSpaceString)}')");
    }
}
