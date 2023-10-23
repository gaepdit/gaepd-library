using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace GaEpd.FileService.Utilities;

[DebuggerStepThrough]
internal static class Guard
{
    internal static void NotNullOrWhiteSpace([NotNull] string? value,
        [CallerArgumentExpression("value")] string? parameterName = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty, or white space.", parameterName);
    }
}
