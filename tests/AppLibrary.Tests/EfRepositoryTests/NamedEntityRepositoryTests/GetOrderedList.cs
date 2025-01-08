using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests.NamedEntityRepositoryTests;

public class GetOrderedList : NamedRepositoryTestBase
{
    [Test]
    public async Task GetOrderedList_ReturnsAllEntities()
    {
        var expected = Repository.Context.Set<TestNamedEntity>().OrderBy(entity => entity.Name);

        var result = await Repository.GetOrderedListAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await Helper.ClearTableAsync<TestNamedEntity>();

        var result = await Repository.GetOrderedListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetOrderedList_UsingPredicate_ReturnsCorrectEntities()
    {
        var expected = Repository.Context.Set<TestNamedEntity>()
            .Where(entity => entity.Name.Contains(EfRepositoryTestHelper.UsefulSuffix))
            .OrderBy(entity => entity.Name);

        var result =
            await Repository.GetOrderedListAsync(entity => entity.Name.Contains(EfRepositoryTestHelper.UsefulSuffix));

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetOrderedList_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await Repository.GetOrderedListAsync(entity => entity.Id == Guid.Empty);

        result.Should().BeEmpty();
    }
}
