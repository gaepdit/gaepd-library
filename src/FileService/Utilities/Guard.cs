using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GaEpd.FileService;

[DebuggerStepThrough]
public static class Guard
{
    public static string NotNullOrWhiteSpace([NotNull] string? value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty, or white space.", parameterName);

        return value;
    }
}
