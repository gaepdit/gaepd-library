using AppLibrary.Tests.EntityHelpers;

namespace AppLibrary.Tests.EfRepositoryTests.NamedEntityRepositoryTests;

public class GetOrderedList
{
    private EfRepositoryTestHelper _helper = default!;

    private DerivedEfNamedEntityRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetNamedEntityRepository();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _repository.DisposeAsync();
        await _helper.DisposeAsync();
    }

    [Test]
    public async Task GetOrderedListAsync_ReturnsAllEntities()
    {
        var expected = _repository.Context.Set<DerivedNamedEntity>().OrderBy(entity => entity.Name);

        var result = await _repository.GetOrderedListAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await _helper.ClearTableAsync<DerivedNamedEntity>();

        var result = await _repository.GetOrderedListAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetOrderedListAsync_UsingPredicate_ReturnsCorrectEntities()
    {
        var expected = _repository.Context.Set<DerivedNamedEntity>()
            .Where(entity => entity.Name.Contains(EfRepositoryTestHelper.UsefulSuffix))
            .OrderBy(entity => entity.Name);

        var result =
            await _repository.GetOrderedListAsync(entity => entity.Name.Contains(EfRepositoryTestHelper.UsefulSuffix));

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetOrderedListAsync_UsingPredicate_WhenNoItemsMatch_ReturnsEmptyList()
    {
        var result = await _repository.GetOrderedListAsync(entity => entity.Id == Guid.Empty);

        result.Should().BeEmpty();
    }
}
