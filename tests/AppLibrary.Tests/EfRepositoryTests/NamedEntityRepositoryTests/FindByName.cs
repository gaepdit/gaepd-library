using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests.NamedEntityRepositoryTests;

public class FindByName : NamedRepositoryTestBase
{
    [Test]
    public async Task FindByName_WhenEntityExists_ReturnsEntity()
    {
        var expected = Repository.Context.Set<TestNamedEntity>().First();

        var result = await Repository.FindByNameAsync(expected.Name);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindByName_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await Repository.FindByNameAsync("xxx");

        result.Should().BeNull();
    }

    [Test]
    public async Task FindByName_IsNotCaseSensitive()
    {
        var expected = Repository.Context.Set<TestNamedEntity>().First();

        var result = await Repository.FindByNameAsync(expected.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(expected);
    }
}
