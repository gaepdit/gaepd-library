using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GaEpd.AppLibrary.GuardClauses;

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
        // The original parameter name should be passed through so that the correct name is returned to
        // the original caller.
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
        // The original parameter name should be passed through so that the correct name is returned to
        // the original caller.
        NotNullOrWhiteSpace(value, parameterName);

        if (minLength > maxLength)
            throw new ArgumentException(
                $"The minimum length '{minLength}' cannot exceed the maximum length '{maxLength}'.", nameof(minLength));

        if (value.Length > maxLength)
            throw new ArgumentException(
                $"The length cannot exceed the maximum length '{maxLength}'.", parameterName);

        if (value.Length < minLength)
            throw new ArgumentException(
                $"The length must be at least the minimum length '{minLength}'.", parameterName);

        return value;
    }

    public static int NotNegative(
        int value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value < 0) 
            throw new ArgumentException("Value cannot be negative.", parameterName);

        return value;
    }

    public static int Positive(
        int value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value <= 0) 
            throw new ArgumentException("Value must be positive (greater than zero).", parameterName);

        return value;
    }

    public static string? RegexMatch(
        string? value,
        string pattern,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value is null) return null;

        if (!Regex.IsMatch(value, pattern, RegexOptions.NonBacktracking))
            throw new ArgumentException("Value does not match regex.", parameterName);

        return value;
    }
}
