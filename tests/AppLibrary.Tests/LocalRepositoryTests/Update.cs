using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class Update : RepositoryTestBase
{
    [Test]
    public async Task Update_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = Repository.Items.First();
        var newEntityWithSameId = new TestEntity { Id = originalEntity.Id, Note = "Xyz" };

        await Repository.UpdateAsync(newEntityWithSameId);

        var result = await Repository.GetAsync(newEntityWithSameId.Id);

        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(newEntityWithSameId);
        Repository.Items.Contains(originalEntity).Should().BeFalse();
    }

    [Test]
    public void Update_WhenItemDoesNotExist_Throws()
    {
        var item = new TestEntity { Id = Guid.NewGuid() };

        var func = async () => await Repository.UpdateAsync(item);

        func.Should().ThrowAsync<EntityNotFoundException<TestEntity>>()
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {item.Id}");
    }
}
