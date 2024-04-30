namespace AppLibrary.Tests.LocalNamedEntityRepositoryTests;

public class GetOrderedList : LocalNamedRepositoryTestBase
{
    [Test]
    public async Task GetOrderedListAsync_ReturnsAllEntities()
    {
        var result = await NamedRepository.GetOrderedListAsync();

        result.Should().BeEquivalentTo(NamedRepository.Items.OrderBy(entity => entity.Name));
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        NamedRepository.Items.Clear();

        var result = await NamedRepository.GetOrderedListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetOrderedListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        var expected = NamedRepository.Items
            .Where(entity => entity.Name.Contains(DerivedLocalNamedEntityRepository.UsefulSuffix))
            .OrderBy(entity => entity.Name);

        var result = await NamedRepository.GetOrderedListAsync(entity => entity.Name.Contains("def"));

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetOrderedListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await NamedRepository.GetOrderedListAsync(entity => entity.Id == Guid.Empty);

        result.Should().BeEmpty();
    }
}
