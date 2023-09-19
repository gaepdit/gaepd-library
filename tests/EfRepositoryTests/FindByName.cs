using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class FindByName
{
    private EfRepositoryTestHelper _helper = default!;

    private EfNamedEntityRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetEfNamedEntityRepository();
    }

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task FindByName_WhenEntityExists_ReturnsEntity()
    {
        var entity = _repository.Context.Set<TestNamedEntity>().First();

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
        var entity = _repository.Context.Set<TestNamedEntity>().First();

        var result = await _repository.FindByNameAsync(entity.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(entity);
    }
}
