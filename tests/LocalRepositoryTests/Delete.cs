namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Delete: LocalRepositoryTestBase
{
    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ShouldDecreaseCountByOne()
    {
        var initialCount = Repository.Items.Count;
        var entity = Repository.Items.First();

        await Repository.DeleteAsync(entity);

        Repository.Items.Count.Should().Be(initialCount - 1);
    }

    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ItemShouldNoLongerExist()
    {
        var entity = Repository.Items.First();

        await Repository.DeleteAsync(entity);

        (await Repository.FindAsync(entity.Id)).Should().BeNull();
    }
}
