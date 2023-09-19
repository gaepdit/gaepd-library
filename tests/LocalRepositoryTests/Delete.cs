using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Delete
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ShouldDecreaseCountByOne()
    {
        var initialCount = _repository.Items.Count;
        var entity = _repository.Items.First();

        await _repository.DeleteAsync(entity);

        _repository.Items.Count.Should().Be(initialCount - 1);
    }

    [Test]
    public async Task DeleteAsync_DeleteExistingItem_ItemShouldNoLongerExist()
    {
        var entity = _repository.Items.First();

        await _repository.DeleteAsync(entity);

        (await _repository.FindAsync(entity.Id)).Should().BeNull();
    }
}
