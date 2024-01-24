// ReSharper disable ConvertIfStatementToReturnStatement

namespace GaEpd.FileService.Utilities;

public static class PathTool
{
    private const string DirectorySeparator = "/";

    public static string Combine(string first, string second, string third = "")
    {
        if (string.IsNullOrEmpty(first)) return CombineInternal(second, third);
        if (string.IsNullOrEmpty(second)) return CombineInternal(first, third);
        if (string.IsNullOrEmpty(third)) return CombineInternal(first, second);

        return JoinInternal(first.AsSpan(), second.AsSpan(), third.AsSpan());
    }

    private static string CombineInternal(string first, string second)
    {
        if (string.IsNullOrEmpty(first)) return second;
        if (string.IsNullOrEmpty(second)) return first;

        return JoinInternal(first.AsSpan(), second.AsSpan());
    }

    private static string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second, ReadOnlySpan<char> third) =>
        string.Concat(
            string.Concat(Path.TrimEndingDirectorySeparator(first), DirectorySeparator.AsSpan()),
            Path.TrimEndingDirectorySeparator(second), DirectorySeparator.AsSpan(),
            Path.TrimEndingDirectorySeparator(third)
        );

    private static string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second) =>
        string.Concat(Path.TrimEndingDirectorySeparator(first), DirectorySeparator.AsSpan(),
            Path.TrimEndingDirectorySeparator(second));

    public static string CombineWithDirectorySeparator(string path) =>
        string.Concat(Path.TrimEndingDirectorySeparator(path), DirectorySeparator);

    public static string CombineWithDirectorySeparator(string first, string second, string third = "") =>
        string.Concat(Combine(first, second, third), DirectorySeparator);
}
