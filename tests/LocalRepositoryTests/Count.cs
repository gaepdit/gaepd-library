using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Count
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task CountAsync_WithoutPredicate_ReturnsCorrectCount()
    {
        var result = await _repository.CountAsync(e => true);

        result.Should().Be(_repository.Items.Count);
    }

    [Test]
    public async Task CountAsync_WithoutPredicate_WhenNoItemsExist_ReturnsZero()
    {
        _repository.Items.Clear();

        var result = await _repository.CountAsync(e => true);

        result.Should().Be(0);
    }

    [Test]
    public async Task CountAsync_WithPredicate_ReturnsCorrectCount()
    {
        // Assuming this is the count you are expecting from your predicate.
        var selectedItemsCount = _repository.Items.Count - 1;

        var result = await _repository.CountAsync(e => e.Id != _repository.Items.First().Id);

        result.Should().Be(selectedItemsCount);
    }

    [Test]
    public async Task CountAsync_WithPredicateThatDoesNotMatchAnyEntity_ReturnsZero()
    {
        var result = await _repository.CountAsync(e => e.Id == Guid.NewGuid());

        result.Should().Be(0);
    }
}
