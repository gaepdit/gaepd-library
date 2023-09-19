using GaEpd.AppLibrary.Tests.EntityHelpers;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Insert
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task InsertAsync_AddNewItem_ShouldIncreaseCountByOne()
    {
        var initialCount = _repository.Items.Count;
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await _repository.InsertAsync(entity);

        _repository.Items.Count.Should().Be(initialCount + 1);
    }

    [Test]
    public async Task InsertAsync_AddNewItem_ShouldBeAbleToRetrieveNewItem()
    {
        var entity = new TestEntity { Id = Guid.NewGuid() };

        await _repository.InsertAsync(entity);
        var result = await _repository.GetAsync(entity.Id);

        result.Should().BeEquivalentTo(entity);
    }
}
