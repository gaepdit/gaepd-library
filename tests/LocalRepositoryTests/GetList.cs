namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class GetList : LocalRepositoryTestBase
{
    [Test]
    public async Task GetListAsync_ReturnsAllEntities()
    {
        var result = await Repository.GetListAsync();

        result.Should().BeEquivalentTo(Repository.Items);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        Repository.Items.Clear();

        var result = await Repository.GetListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        // Assuming this predicate selects correct items.
        var selectedItems = Repository.Items.Skip(1).ToList();

        var result = await Repository.GetListAsync(e => e.Id == selectedItems[0].Id);

        result.Should().BeEquivalentTo(selectedItems);
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await Repository.GetListAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeEmpty();
    }
}
