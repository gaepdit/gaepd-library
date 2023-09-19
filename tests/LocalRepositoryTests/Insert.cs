using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Insert : LocalRepositoryTestBase
{
    [Test]
    public async Task InsertAsync_AddNewItem_ShouldIncreaseCountByOne()
    {
        var initialCount = Repository.Items.Count;
        var entity = new DerivedEntity { Id = Guid.NewGuid() };

        await Repository.InsertAsync(entity);

        Repository.Items.Count.Should().Be(initialCount + 1);
    }

    [Test]
    public async Task InsertAsync_AddNewItem_ShouldBeAbleToRetrieveNewItem()
    {
        var entity = new DerivedEntity { Id = Guid.NewGuid() };

        await Repository.InsertAsync(entity);
        var result = await Repository.GetAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }
}
