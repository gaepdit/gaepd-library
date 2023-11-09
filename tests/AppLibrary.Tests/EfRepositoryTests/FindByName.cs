using AppLibrary.Tests.EntityHelpers;
using AppLibrary.Tests.RepositoryHelpers;

namespace AppLibrary.Tests.EfRepositoryTests;

public class FindByName
{
    private EfRepositoryTestHelper _helper = default!;

    private DerivedEfNamedEntityRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetNamedEntityRepository();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _repository.DisposeAsync();
        await _helper.DisposeAsync();
    }

    [Test]
    public async Task FindByName_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Context.Set<DerivedNamedEntity>().First();

        var result = await _repository.FindByNameAsync(entity.Name);

        result.Should().BeEquivalentTo(entity);
    }

    [Test]
    public async Task FindByName_WhenEntityDoesNotExist_ReturnsNull()
    {
        var entity = await _repository.FindByNameAsync("xxx");

        entity.Should().BeNull();
    }

    [Test]
    public async Task FindByName_IsNotCaseSensitive()
    {
        var entity = _repository.Context.Set<DerivedNamedEntity>().First();

        var result = await _repository.FindByNameAsync(entity.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(entity);
    }
}
