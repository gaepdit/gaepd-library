using AppLibrary.Tests.TestEntities;

namespace AppLibrary.Tests.EfRepositoryTests;

public class Find : RepositoryTestBase
{
    [Test]
    public async Task Find_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.FindAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task Find_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await Repository.FindAsync(id);

        result.Should().BeNull();
    }

    [Test]
    public async Task Find_UsingPredicate_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        var result = await Repository.FindAsync(e => e.Id == entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task Find_UsingPredicate_WhenEntityDoesNotExist_ReturnsNull()
    {
        var id = Guid.NewGuid();

        var result = await Repository.FindAsync(e => e.Id == id);

        result.Should().BeNull();
    }

    [Test]
    public async Task Find_UsingPredicate_WhenUsingSqlite_IsCaseSensitive()
    {
        var entity = Repository.Context.Set<TestEntity>().First();

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await Repository.FindAsync(e =>
            e.Note.ToUpper().Equals(entity.Note.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await Repository.FindAsync(e =>
            e.Note.ToUpper().Equals(entity.Note.ToLower()));

        using var scope = new AssertionScope();
        resultSameCase.Should().BeEquivalentTo(entity);
        resultDifferentCase.Should().BeNull();
    }

    [Test]
    public async Task Find_UsingPredicate_WhenUsingSqlServer_IsNotCaseSensitive()
    {
        await using var repositoryHelper = EfRepositoryTestHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetRepository();
        var entity = repository.Context.Set<TestEntity>().First();

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await repository.FindAsync(e =>
            e.Note.ToUpper().Equals(entity.Note.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await repository.FindAsync(e =>
            e.Note.ToUpper().Equals(entity.Note.ToLower()));

        using var scope = new AssertionScope();
        resultSameCase.Should().BeEquivalentTo(entity);
        resultDifferentCase.Should().BeEquivalentTo(entity);
    }
}
