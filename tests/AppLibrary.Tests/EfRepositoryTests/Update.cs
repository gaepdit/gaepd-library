using AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Update : EfRepositoryTestBase
{
    [Test]
    public async Task UpdateAsync_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = Repository.Context.Set<DerivedEntity>().First();
        Helper.ClearChangeTracker();
        var newEntityWithSameId = new DerivedEntity { Id = originalEntity.Id, Note = "Xyz" };

        await Repository.UpdateAsync(newEntityWithSameId);

        Helper.ClearChangeTracker();
        var result = await Repository.GetAsync(newEntityWithSameId.Id);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(newEntityWithSameId);
            Repository.Context.Set<DerivedEntity>().ToList().Contains(originalEntity).Should().BeFalse();
        }
    }

    [Test]
    public void UpdateAsync_WhenItemDoesNotExist_Throws()
    {
        var item = new DerivedEntity { Id = Guid.NewGuid() };

        var func = async () => await Repository.UpdateAsync(item);

        func.Should().ThrowAsync<EntityNotFoundException<DerivedEntity>>()
            .WithMessage($"Entity not found. Entity type: {typeof(DerivedEntity).FullName}, id: {item.Id}");
    }
}
