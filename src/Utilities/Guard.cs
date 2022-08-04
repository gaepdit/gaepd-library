using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GaEpd.Library.Utilities;

[DebuggerStepThrough]
public static class Guard
{
    public static T NotNull<T>(
        [NotNull] T value,
        [CallerArgumentExpression("value")] string? parameterName = null) =>
        value ?? throw new ArgumentNullException(parameterName);

    public static string NotNullOrWhiteSpace(
        [NotNull] string? value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        NotNull(value, parameterName);

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty, or white space.", parameterName);

        return value;
    }

    public static string ValidLength(
        [NotNull] string? value,
        int minLength = 0,
        int maxLength = int.MaxValue,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        NotNullOrWhiteSpace(value, parameterName);

        if (minLength > maxLength)
            throw new ArgumentException(
                $"The minimum length '{minLength}' cannot exceed the maximum length '{maxLength}'.", nameof(minLength));

        if (value.Length > maxLength)
            throw new ArgumentException($"The length cannot exceed the maximum length '{maxLength}'.", parameterName);

        if (minLength > 0 && value.Length < minLength)
            throw new ArgumentException($"The length must be at least the minimum length '{minLength}'.", parameterName);

        return value;
    }

    public static int NotNegative(
        int value,
        [CallerArgumentExpression("value")] string? parameterName = null) =>
        value >= 0
            ? value
            : throw new ArgumentException("Value cannot be negative.", parameterName);

    public static int Positive(
        int value,
        [CallerArgumentExpression("value")] string? parameterName = null) =>
        value > 0
            ? value
            : throw new ArgumentException("Value must be positive (greater than zero).", parameterName);

    public static string? RegexMatch(
        string? value,
        string pattern,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value is null) return null;

        return Regex.IsMatch(value, pattern)
            ? value
            : throw new ArgumentException("Value does not match regex.", parameterName);
    }
}
