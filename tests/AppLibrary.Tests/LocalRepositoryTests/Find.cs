namespace AppLibrary.Tests.LocalRepositoryTests;

public class Find : RepositoryTestBase
{
    [Test]
    public async Task FindAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = Repository.Items.First();

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
        var entity = Repository.Items.First();

        var result = await Repository.FindAsync(e => e.Id == entity.Id);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await Repository.FindAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeNull();
    }

    [Test]
    public async Task FindAsync_UsingPredicate_WhenUsingLocalRepository_IsCaseSensitive()
    {
        var entity = Repository.Items.First();

        var resultIgnoreCase = await Repository.FindAsync(e =>
            e.Note.ToUpperInvariant()
                .Equals(entity.Note.ToLowerInvariant(), StringComparison.CurrentCultureIgnoreCase));

        var resultCaseSensitive = await Repository.FindAsync(e =>
            e.Note.ToUpperInvariant().Equals(entity.Note.ToLowerInvariant(), StringComparison.CurrentCulture));

        using (new AssertionScope())
        {
            resultIgnoreCase.Should().BeEquivalentTo(entity);
            resultCaseSensitive.Should().BeNull();
        }
    }
}
