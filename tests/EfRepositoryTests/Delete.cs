using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Delete
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
    public async Task DeleteAsync_DeleteExistingItem_ShouldDecreaseCountByOne()
    {
        var items = _repository.Context.Set<TestEntity>();
        var initialCount = items.Count();
        var entity = items.First();

        await _repository.DeleteAsync(entity);

        _repository.Context.Set<TestEntity>().Count().Should().Be(initialCount - 1);
    }

    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ItemShouldNoLongerExist()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

        await _repository.DeleteAsync(entity);

        (await _repository.FindAsync(entity.Id)).Should().BeNull();
    }
}
