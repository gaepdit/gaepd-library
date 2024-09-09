using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Get : RepositoryTestBase
{
    [Test]
    public void GetAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        var id = Guid.NewGuid();

        var func = async () => await Repository.GetAsync(id);

        func.Should().ThrowAsync<EntityNotFoundException<TestEntity>>()
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {id}");
    }

    [Test]
    public async Task GetAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.GetAsync(entity.Id);

        result.Should().Be(entity);
    }
}
