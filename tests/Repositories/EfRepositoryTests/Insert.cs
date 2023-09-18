using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Insert
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
    public async Task InsertAsync_AddNewItem_ShouldIncreaseCountByOne()
    {
        var items = _repository.Context.Set<TestEntity>();
        var initialCount = items.Count();
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await _repository.InsertAsync(entity);

        items.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public async Task InsertAsync_AddNewItem_ShouldBeAbleToRetrieveNewItem()
    {
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await _repository.InsertAsync(entity);
        var result = await _repository.GetAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }
}
