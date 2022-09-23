using FluentAssertions;
using GaEpd.Library.GuardClauses;

namespace GaEpd.Library.Tests.GuardTests;

public class RegexMatch
{
    private const string RegexPattern = "^[0-1]+$";

    [Test]
    public void DoesNotThrowGivenValidMatch()
    {
        const string validMatch = "101";
        Action act = () => Guard.RegexMatch(validMatch, RegexPattern);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenValidValue()
    {
        const string validMatch = "101";
        var result = Guard.RegexMatch(validMatch, RegexPattern);
        result.Should().Be(validMatch);
    }

    [Test]
    public void ThrowsGivenInvalidMatch()
    {
        const string invalidMatch = "ABC";
        Action act = () => Guard.RegexMatch(invalidMatch, RegexPattern);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value does not match regex. (Parameter '{nameof(invalidMatch)}')");
    }
}
