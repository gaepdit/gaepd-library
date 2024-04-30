namespace AppLibrary.Tests.LocalNamedEntityRepositoryTests;

public class FindByName : LocalNamedRepositoryTestBase
{
    [Test]
    public async Task FindByName_WhenEntityExists_ReturnsEntity()
    {
        var expected = NamedRepository.Items.First();

        var result = await NamedRepository.FindByNameAsync(expected.Name);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindByName_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await NamedRepository.FindByNameAsync("xxx");

        result.Should().BeNull();
    }

    [Test]
    public async Task FindByName_IsNotCaseSensitive()
    {
        var expected = NamedRepository.Items.First();

        var result = await NamedRepository.FindByNameAsync(expected.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(expected);
    }
}
