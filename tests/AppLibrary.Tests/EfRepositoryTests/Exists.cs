using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Exists : RepositoryTestBase
{
    [Test]
    public async Task ExistsAsync_WhenEntityExists_ReturnsTrue()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.ExistsAsync(entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        var id = Guid.NewGuid();

        var result = await Repository.ExistsAsync(id);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsAsync_UsingPredicate_WhenEntityExists_ReturnsTrue()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.ExistsAsync(e => e.Id == entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsFall()
    {
        var id = Guid.NewGuid();

        var result = await Repository.ExistsAsync(e => e.Id == id);

        result.Should().BeFalse();
    }
}
