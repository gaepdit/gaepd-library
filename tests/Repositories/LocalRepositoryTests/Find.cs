using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Find
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task FindAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Items.First();

        var result = await _repository.FindAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await _repository.FindAsync(id);

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Items.First();

        var result = await _repository.FindAsync(e => e.Id == entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenUsingLocalRepository_IsCaseSensitive()
    {
        var entity = _repository.Items.First();

        var resultIgnoreCase = await _repository.FindAsync(e =>
            e.Name.ToUpperInvariant().Equals(entity.Name.ToLowerInvariant(), StringComparison.CurrentCultureIgnoreCase));

        var resultCaseSensitive = await _repository.FindAsync(e =>
            e.Name.ToUpperInvariant().Equals(entity.Name.ToLowerInvariant(), StringComparison.CurrentCulture));

        using (new AssertionScope())
        {
            resultIgnoreCase.Should().BeEquivalentTo(entity);
            resultCaseSensitive.Should().BeNull();
        }
    }
}
