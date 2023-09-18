using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class GetList
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
    public async Task GetListAsync_ReturnsAllEntities()
    {
        var items = _repository.Context.Set<TestEntity>();

        var result = await _repository.GetListAsync();

        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await _helper.ClearTestEntityTableAsync();

        var result = await _repository.GetListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        // Assuming this predicate selects correct items.
        var items = _repository.Context.Set<TestEntity>();
        var selectedItems = items.Skip(1).ToList();

        var result = await _repository.GetListAsync(e => e.Id == selectedItems[0].Id);

        result.Should().BeEquivalentTo(selectedItems);
    }

    [Test]
    public async Task GetListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var id = Guid.NewGuid();

        var result = await _repository.GetListAsync(e => e.Id == id);

        result.Should().BeEmpty();
    }
}
