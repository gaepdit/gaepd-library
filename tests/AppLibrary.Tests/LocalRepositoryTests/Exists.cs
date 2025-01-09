namespace AppLibrary.Tests.LocalRepositoryTests;

public class Exists : RepositoryTestBase
{
    [Test]
    public async Task Exists_WhenEntityExists_ReturnsTrue()
    {
        var entity = Repository.Items.First();

        var result = await Repository.ExistsAsync(entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_WhenEntityDoesNotExist_ReturnsFalse()
    {
        var id = Guid.NewGuid();

        var result = await Repository.ExistsAsync(id);

        result.Should().BeFalse();
    }

    [Test]
    public async Task Exists_UsingPredicate_WhenEntityExists_ReturnsTrue()
    {
        var entity = Repository.Items.First();

        var result = await Repository.ExistsAsync(e => e.Id == entity.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_UsingPredicate_WhenEntityDoesNotExist_ReturnsFall()
    {
        var result = await Repository.ExistsAsync(e => e.Id == Guid.NewGuid());

        result.Should().BeFalse();
    }
}
