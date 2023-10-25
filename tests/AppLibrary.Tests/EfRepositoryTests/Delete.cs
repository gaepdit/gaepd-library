using AppLibrary.Tests.EntityHelpers;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Delete : EfRepositoryTestBase
{
    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ShouldDecreaseCountByOne()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        var initialCount = items.Count();
        var entity = items.First();

        await Repository.DeleteAsync(entity);

        Repository.Context.Set<DerivedEntity>().Count().Should().Be(initialCount - 1);
    }

    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ItemShouldNoLongerExist()
    {
        var entity = Repository.Context.Set<DerivedEntity>().First();

        await Repository.DeleteAsync(entity);

        (await Repository.FindAsync(entity.Id)).Should().BeNull();
    }
}
