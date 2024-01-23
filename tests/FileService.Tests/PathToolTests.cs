using GaEpd.FileService.Utilities;

namespace FileService.Tests;

public class PathToolTests
{
    [TestCase("a", "", "", "a")]
    [TestCase("", "b", "", "b")]
    [TestCase("", "", "c", "c")]
    [TestCase("a", "b", "", "a/b")]
    [TestCase("a", "b", "c", "a/b/c")]
    [TestCase("a/", "b/", "c/", "a/b/c")]
    [TestCase("a\\", "b\\", "c\\", "a/b/c")]
    public void CombineTest(string first, string second, string third, string expected)
    {
        // Act
        PathTool.Combine(first, second, third)
            // Assert
            .Should().Be(expected);
    }

    [TestCase("a", "", "", "a/")]
    [TestCase("", "b", "", "b/")]
    [TestCase("", "", "c", "c/")]
    [TestCase("a", "b", "", "a/b/")]
    [TestCase("a", "b", "c", "a/b/c/")]
    [TestCase("a/", "b/", "c/", "a/b/c/")]
    [TestCase("a\\", "b\\", "c\\", "a/b/c/")]
    public void CombineWithDirectorySeparatorTest(string first, string second, string third, string expected)
    {
        // Act
        PathTool.CombineWithDirectorySeparator(first, second, third)
            // Assert
            .Should().Be(expected);
    }
}
