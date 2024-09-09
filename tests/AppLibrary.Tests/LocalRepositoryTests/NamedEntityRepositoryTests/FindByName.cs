namespace AppLibrary.Tests.LocalRepositoryTests.NamedEntityRepositoryTests;

public class FindByName : NamedRepositoryTestBase
{
    [Test]
    public async Task FindByName_WhenEntityExists_ReturnsEntity()
    {
        var expected = NamedEntityRepository.Items.First();

        var result = await NamedEntityRepository.FindByNameAsync(expected.Name);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindByName_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await NamedEntityRepository.FindByNameAsync("xxx");

        result.Should().BeNull();
    }

    [Test]
    public async Task FindByName_IsNotCaseSensitive()
    {
        var expected = NamedEntityRepository.Items.First();

        var result = await NamedEntityRepository.FindByNameAsync(expected.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(expected);
    }
}
