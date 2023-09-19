using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class GetList : EfRepositoryTestBase
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
        await Helper.ClearTestEntityTableAsync();

        var result = await Repository.GetListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        // Assuming this predicate selects correct items.
        var items = Repository.Context.Set<TestEntity>();
        var selectedItems = items.Skip(1).ToList();

        var result = await Repository.GetListAsync(e => e.Id == selectedItems[0].Id);

        result.Should().BeEquivalentTo(selectedItems);
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var id = Guid.NewGuid();

        var result = await Repository.GetListAsync(e => e.Id == id);

        result.Should().BeEmpty();
    }
}
