using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Get
{
    private EfRepositoryTestHelper _helper = default!;

    private EfRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetEfRepository();
    }

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        var id = Guid.NewGuid();

        var func = async () => await _repository.GetAsync(id);

        (await func.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {id}");
    }

    [Test]
    public async Task GetAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

        var result = await _repository.GetAsync(entity.Id);

        result.Should().Be(entity);
    }
}
