using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Find
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
    public async Task FindAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

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
        var entity = _repository.Context.Set<TestEntity>().First();

        var result = await _repository.FindAsync(e => e.Id == entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await _repository.FindAsync(e => e.Id == id);

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenUsingSqlite_IsCaseSensitive()
    {
        var entity = _repository.Context.Set<TestEntity>().First();

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await _repository.FindAsync(e =>
            e.Name.ToUpper().Equals(entity.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await _repository.FindAsync(e =>
            e.Name.ToUpper().Equals(entity.Name.ToLower()));

        using (new AssertionScope())
        {
            resultSameCase.Should().BeEquivalentTo(entity);
            resultDifferentCase.Should().BeNull();
        }
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenUsingSqlServer_IsNotCaseSensitive()
    {
        using var repositoryHelper = EfRepositoryTestHelper.CreateSqlServerRepositoryHelper(this);
        using var repository = repositoryHelper.GetEfRepository();
        var entity = repository.Context.Set<TestEntity>().First();

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(entity.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(entity.Name.ToLower()));

        using (new AssertionScope())
        {
            resultSameCase.Should().BeEquivalentTo(entity);
            resultDifferentCase.Should().BeEquivalentTo(entity);
        }
    }
}
