using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Update : RepositoryTestBase
{
    [Test]
    public async Task UpdateAsync_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = Repository.Context.Set<TestEntity>().First();
        Helper.ClearChangeTracker();
        var newEntityWithSameId = new TestEntity { Id = originalEntity.Id, Note = "Xyz" };

        await Repository.UpdateAsync(newEntityWithSameId);

        Helper.ClearChangeTracker();
        var result = await Repository.GetAsync(newEntityWithSameId.Id);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(newEntityWithSameId);
            Repository.Context.Set<TestEntity>().ToList().Contains(originalEntity).Should().BeFalse();
        }
    }

    [Test]
    public void UpdateAsync_WhenItemDoesNotExist_Throws()
    {
        var item = new TestEntity { Id = Guid.NewGuid() };

        var func = async () => await Repository.UpdateAsync(item);

        func.Should().ThrowAsync<EntityNotFoundException<TestEntity>>()
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {item.Id}");
    }
}
