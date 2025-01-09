using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class GetListWithOrdering : RepositoryTestBase
{
    [Test]
    public async Task GetListAsc_ReturnsAllEntitiesInAscendingOrder()
    {
        var items = Repository.Context.Set<TestEntity>().OrderBy(entity => entity.Note);

        var result = await Repository.GetListAsync(ordering: "Note");

        result.Should().BeEquivalentTo(items, options => options.WithStrictOrdering());
    }

    [Test]
    public async Task GetListDesc_ReturnsAllEntitiesInDescendingOrder()
    {
        var items = Repository.Context.Set<TestEntity>().OrderByDescending(entity => entity.Note);

        var result = await Repository.GetListAsync(ordering: "Note desc");

        result.Should().BeEquivalentTo(items, options => options.WithStrictOrdering());
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await Helper.ClearTableAsync<TestEntity>();

        var result = await Repository.GetListAsync(ordering: "Note");

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsc_UsingPredicate_ReturnsCorrectEntitiesInAscendingOrder()
    {
        var skipId = Repository.Context.Set<TestEntity>().First().Id;
        var expected = Repository.Context.Set<TestEntity>().Where(entity => entity.Id != skipId)
            .OrderBy(entity => entity.Note);

        var result = await Repository.GetListAsync(entity => entity.Id != skipId, ordering: "Note");

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetListDesc_UsingPredicate_ReturnsCorrectEntitiesInDescendingOrder()
    {
        var skipId = Repository.Context.Set<TestEntity>().First().Id;
        var expected = Repository.Context.Set<TestEntity>().Where(entity => entity.Id != skipId)
            .OrderByDescending(entity => entity.Note);

        var result = await Repository.GetListAsync(entity => entity.Id != skipId, ordering: "Note desc");

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetList_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await Repository.GetListAsync(entity => entity.Id == Guid.Empty, ordering: "Note");

        result.Should().BeEmpty();
    }
}
