namespace AppLibrary.Tests.LocalRepositoryTests.NamedEntityRepositoryTests;

public class GetOrderedList : NamedRepositoryTestBase
{
    [Test]
    public async Task GetOrderedList_ReturnsAllEntities()
    {
        var result = await NamedEntityRepository.GetOrderedListAsync();

        result.Should().BeEquivalentTo(NamedEntityRepository.Items.OrderBy(entity => entity.Name));
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        NamedEntityRepository.Items.Clear();

        var result = await NamedEntityRepository.GetOrderedListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetOrderedList_UsingPredicate_ReturnsCorrectEntities()
    {
        var expected = NamedEntityRepository.Items
            .Where(entity => entity.Name.Contains(TestNamedEntityRepository.UsefulSuffix))
            .OrderBy(entity => entity.Name);

        var result = await NamedEntityRepository.GetOrderedListAsync(entity => entity.Name.Contains("def"));

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetOrderedList_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await NamedEntityRepository.GetOrderedListAsync(entity => entity.Id == Guid.Empty);

        result.Should().BeEmpty();
    }
}
