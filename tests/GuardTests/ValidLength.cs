using FluentAssertions;
using GaEpd.Library.GuardClauses;

namespace GaEpd.Library.Tests.GuardTests;

public class ValidLength
{
    [Test]
    public void DoesNotThrowGivenValidValue()
    {
        const string validValue = "a";
        Action act = () => Guard.ValidLength(validValue);
        act.Should().NotThrow();
    }

    [Test]
    public void ReturnsSameValueGivenValidValue()
    {
        const string validValue = "a";
        var resultString = Guard.ValidLength(validValue);
        resultString.Should().Be(validValue);
    }

    [Test]
    public void ThrowsGivenNullValue()
    {
        const string? nullString = null;
        Action act = () => Guard.ValidLength(nullString);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{nameof(nullString)}')");
    }

    [Test]
    public void ThrowsGivenWhiteSpaceValue()
    {
        const string? whiteSpaceString = " ";
        Action act = () => Guard.ValidLength(whiteSpaceString);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Value cannot be null, empty, or white space. (Parameter '{nameof(whiteSpaceString)}')");
    }

    [Test]
    public void ThrowsIfMinLengthExceedsMaxLength()
    {
        const string value = "a";
        const int minLength = 2;
        const int maxLength = 1;
        Action act = () => Guard.ValidLength(value, minLength: minLength, maxLength: maxLength);
        act.Should().Throw<ArgumentException>()
            .WithMessage(
                $"The minimum length '{minLength}' cannot exceed the maximum length '{maxLength}'. (Parameter '{nameof(minLength)}')");
    }

    [Test]
    public void ThrowsGivenValueThatIsTooLong()
    {
        const string? longString = "ab";
        const int maxLength = 1;
        Action act = () => Guard.ValidLength(longString, maxLength: maxLength);
        act.Should().Throw<ArgumentException>()
            .WithMessage(
                $"The length cannot exceed the maximum length '{maxLength}'. (Parameter '{nameof(longString)}')");
    }

    [Test]
    public void ThrowsGivenValueThatIsTooShort()
    {
        const string? shortString = "a";
        const int minLength = 2;
        Action act = () => Guard.ValidLength(shortString, minLength: minLength);
        act.Should().Throw<ArgumentException>()
            .WithMessage(
                $"The length must be at least the minimum length '{minLength}'. (Parameter '{nameof(shortString)}')");
    }
}
