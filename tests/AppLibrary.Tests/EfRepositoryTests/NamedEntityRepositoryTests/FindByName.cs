using AppLibrary.Tests.EntityHelpers;

namespace AppLibrary.Tests.EfRepositoryTests.NamedEntityRepositoryTests;

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
        var expected = _repository.Context.Set<DerivedNamedEntity>().First();

        var result = await _repository.FindByNameAsync(expected.Name);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task FindByName_WhenEntityDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync("xxx");

        result.Should().BeNull();
    }

    [Test]
    public async Task FindByName_IsNotCaseSensitive()
    {
        var expected = _repository.Context.Set<DerivedNamedEntity>().First();

        var result = await _repository.FindByNameAsync(expected.Name.ToUpperInvariant());

        result.Should().BeEquivalentTo(expected);
    }
}
