using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Insert : EfRepositoryTestBase
{
    [Test]
    public async Task InsertAsync_AddNewItem_ShouldIncreaseCountByOne()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        var initialCount = items.Count();
        var entity = new DerivedEntity { Id = Guid.NewGuid() };

        await Repository.InsertAsync(entity);

        items.Count().Should().Be(initialCount + 1);
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
