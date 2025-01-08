using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Insert : RepositoryTestBase
{
    [Test]
    public async Task Insert_AddNewItem_ShouldIncreaseCountByOne()
    {
        var items = Repository.Context.Set<TestEntity>();
        var initialCount = items.Count();
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await Repository.InsertAsync(entity);

        items.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public async Task Insert_AddNewItem_ShouldBeAbleToRetrieveNewItem()
    {
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await Repository.InsertAsync(entity);
        var result = await Repository.GetAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }
}
