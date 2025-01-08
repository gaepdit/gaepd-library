namespace AppLibrary.Tests.LocalRepositoryTests;

public class Delete : RepositoryTestBase
{
    [Test]
    public async Task Delete_DeleteExistingItem_ShouldDecreaseCountByOne()
    {
        var initialCount = Repository.Items.Count;
        var entity = Repository.Items.First();

        await Repository.DeleteAsync(entity);

        Repository.Items.Count.Should().Be(initialCount - 1);
    }

    [Test]
    public async Task Delete_DeleteExistingItem_ItemShouldNoLongerExist()
    {
        var entity = Repository.Items.First();

        await Repository.DeleteAsync(entity);

        (await Repository.FindAsync(entity.Id)).Should().BeNull();
    }
}
