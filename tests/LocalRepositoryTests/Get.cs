using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Get
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

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
        var entity = _repository.Items.First();

        var result = await _repository.GetAsync(entity.Id);

        result.Should().Be(entity);
    }
}
