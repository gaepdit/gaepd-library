using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Exists
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task ExistsAsync_WhenEntityExists_ReturnsTrue()
    {
        var entity = _repository.Items.First();

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
        var entity = _repository.Items.First();

        var result = await _repository.ExistsAsync(e => e.Id == entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsFall()
    {
        var result = await _repository.ExistsAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeFalse();
    }
}
