using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class GetList : RepositoryTestBase
{
    [Test]
    public async Task GetListAsync_ReturnsAllEntities()
    {
        var items = Repository.Context.Set<TestEntity>();

        var result = await Repository.GetListAsync();

        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await Helper.ClearTableAsync<TestEntity>();

        var result = await Repository.GetListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        var skipId = Repository.Context.Set<TestEntity>().First().Id;
        var expected = Repository.Context.Set<TestEntity>().Where(entity => entity.Id != skipId);

        var result = await Repository.GetListAsync(entity => entity.Id != skipId);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await Repository.GetListAsync(entity => entity.Id == Guid.Empty);

        result.Should().BeEmpty();
    }
}
