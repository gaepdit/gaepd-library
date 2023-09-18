using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Update
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
    public async Task UpdateAsync_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = _repository.Context.Set<TestEntity>().First();
        _helper.ClearChangeTracker();
        var newEntityWithSameId = new TestEntity { Id = originalEntity.Id, Name = "Xyz" };

        await _repository.UpdateAsync(newEntityWithSameId);

        _helper.ClearChangeTracker();
        var result = await _repository.GetAsync(newEntityWithSameId.Id);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(newEntityWithSameId);
            _repository.Context.Set<TestEntity>().ToList().Contains(originalEntity).Should().BeFalse();
        }
    }

    [Test]
    public async Task UpdateAsync_WhenItemDoesNotExist_Throws()
    {
        var item = new TestEntity { Id = Guid.NewGuid() };

        var action = async () => await _repository.UpdateAsync(item);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {item.Id}");
    }
}
