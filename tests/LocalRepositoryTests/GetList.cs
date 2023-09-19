using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class GetList
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetListAsync_ReturnsAllEntities()
    {
        var result = await _repository.GetListAsync();

        result.Should().BeEquivalentTo(_repository.Items);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        _repository.Items.Clear();

        var result = await _repository.GetListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        // Assuming this predicate selects correct items.
        var selectedItems = _repository.Items.Skip(1).ToList();

        var result = await _repository.GetListAsync(e => e.Id == selectedItems[0].Id);

        result.Should().BeEquivalentTo(selectedItems);
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeEmpty();
    }
}
