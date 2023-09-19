using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Exists
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
    public async Task ExistsAsync_WhenEntityExists_ReturnsTrue()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

        var result = await _repository.ExistsAsync(entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        var id = Guid.NewGuid();

        var result = await _repository.ExistsAsync(id);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsAsync_UsingPredicate_WhenEntityExists_ReturnsTrue()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

        var result = await _repository.ExistsAsync(e => e.Id == entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsFall()
    {
        var id = Guid.NewGuid();

        var result = await _repository.ExistsAsync(e => e.Id == id);

        result.Should().BeFalse();
    }
}
