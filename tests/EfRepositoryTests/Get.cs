using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.EntityHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Get : EfRepositoryTestBase
{
    [Test]
    public async Task GetAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        var id = Guid.NewGuid();

        var func = async () => await Repository.GetAsync(id);

        (await func.Should().ThrowAsync<EntityNotFoundException>())
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
