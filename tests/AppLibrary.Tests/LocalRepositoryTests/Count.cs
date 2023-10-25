namespace AppLibrary.Tests.LocalRepositoryTests;

public class Count : LocalRepositoryTestBase
{
    [Test]
    public async Task CountAsync_WithoutPredicate_ReturnsCorrectCount()
    {
        var result = await Repository.CountAsync(e => true);

        result.Should().Be(Repository.Items.Count);
    }

    [Test]
    public async Task CountAsync_WithoutPredicate_WhenNoItemsExist_ReturnsZero()
    {
        Repository.Items.Clear();

        var result = await Repository.CountAsync(e => true);

        result.Should().Be(0);
    }

    [Test]
    public async Task CountAsync_WithPredicate_ReturnsCorrectCount()
    {
        // Assuming this is the count you are expecting from your predicate.
        var selectedItemsCount = Repository.Items.Count - 1;

        var result = await Repository.CountAsync(e => e.Id != Repository.Items.First().Id);

        result.Should().Be(selectedItemsCount);
    }

    [Test]
    public async Task CountAsync_WithPredicateThatDoesNotMatchAnyEntity_ReturnsZero()
    {
        var result = await Repository.CountAsync(e => e.Id == Guid.NewGuid());

        result.Should().Be(0);
    }
}
