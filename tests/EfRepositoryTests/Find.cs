using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class Find : EfRepositoryTestBase
{
    [Test]
    public async Task FindAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.FindAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await Repository.FindAsync(id);

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.FindAsync(e => e.Id == entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await Repository.FindAsync(e => e.Id == id);

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenUsingSqlite_IsCaseSensitive()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await Repository.FindAsync(e =>
            e.Name.ToUpper().Equals(entity.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await Repository.FindAsync(e =>
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
