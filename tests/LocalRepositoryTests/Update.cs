using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Update : LocalRepositoryTestBase
{
    [Test]
    public async Task UpdateAsync_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = Repository.Items.First();
        var newEntityWithSameId = new TestEntity { Id = originalEntity.Id, Name = "Xyz" };

        await Repository.UpdateAsync(newEntityWithSameId);

        var result = await Repository.GetAsync(newEntityWithSameId.Id);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(newEntityWithSameId);
            Repository.Items.Contains(originalEntity).Should().BeFalse();
        }
    }

    [Test]
    public async Task UpdateAsync_WhenItemDoesNotExist_Throws()
    {
        var item = new TestEntity { Id = Guid.NewGuid() };

        var action = async () => await Repository.UpdateAsync(item);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {item.Id}");
    }
}
