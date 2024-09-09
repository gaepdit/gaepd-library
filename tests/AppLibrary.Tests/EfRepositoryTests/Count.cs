using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Count : RepositoryTestBase
{
    [Test]
    public async Task CountAsync_WithoutPredicate_ReturnsCorrectCount()
    {
        var result = await Repository.CountAsync(e => true);

        result.Should().Be(Repository.Context.Set<TestEntity>().Count());
    }

    [Test]
    public async Task CountAsync_WithoutPredicate_WhenNoItemsExist_ReturnsZero()
    {
        await Helper.ClearTableAsync<TestEntity>();

        var result = await Repository.CountAsync(e => true);

        result.Should().Be(0);
    }

    [Test]
    public async Task CountAsync_WithPredicate_ReturnsCorrectCount()
    {
        // Assuming this is the count you are expecting from your predicate.
        var selectedItemsCount = Repository.Context.Set<TestEntity>().Count() - 1;

        var result = await Repository.CountAsync(e => e.Id != Repository.Context.Set<TestEntity>().First().Id);

        result.Should().Be(selectedItemsCount);
    }

    [Test]
    public async Task CountAsync_WithPredicateThatDoesNotMatchAnyEntity_ReturnsZero()
    {
        var id = Guid.NewGuid();

        var result = await Repository.CountAsync(e => e.Id == id);

        result.Should().Be(0);
    }
}
