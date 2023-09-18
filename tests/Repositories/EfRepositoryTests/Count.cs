using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Count
{
    private EfRepositoryTestHelper _helper = default!;

    private EfRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetEfRepository();
    }

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task CountAsync_WithoutPredicate_ReturnsCorrectCount()
    {
        var result = await _repository.CountAsync(e => true);

        result.Should().Be(_repository.Context.Set<TestEntity>().Count());
    }

    [Test]
    public async Task CountAsync_WithoutPredicate_WhenNoItemsExist_ReturnsZero()
    {
        await _helper.ClearTestEntityTableAsync();

        var result = await _repository.CountAsync(e => true);

        result.Should().Be(0);
    }

    [Test]
    public async Task CountAsync_WithPredicate_ReturnsCorrectCount()
    {
        // Assuming this is the count you are expecting from your predicate.
        var selectedItemsCount = _repository.Context.Set<TestEntity>().Count() - 1;

        var result = await _repository.CountAsync(e => e.Id != _repository.Context.Set<TestEntity>().First().Id);

        result.Should().Be(selectedItemsCount);
    }

    [Test]
    public async Task CountAsync_WithPredicateThatDoesNotMatchAnyEntity_ReturnsZero()
    {
        var id = Guid.NewGuid();

        var result = await _repository.CountAsync(e => e.Id == id);

        result.Should().Be(0);
    }
}
