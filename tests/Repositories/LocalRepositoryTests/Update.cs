using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class Update
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task UpdateAsync_UpdateExistingItem_ShouldReflectChanges()
    {
        var originalEntity = _repository.Items.First();
        var newEntityWithSameId = new TestEntity { Id = originalEntity.Id, Name = "Xyz" };

        await _repository.UpdateAsync(newEntityWithSameId);

        var result = await _repository.GetAsync(newEntityWithSameId.Id);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(newEntityWithSameId);
            _repository.Items.Contains(originalEntity).Should().BeFalse();
        }
    }

    [Test]
    public async Task UpdateAsync_WhenItemDoesNotExist_Throws()
    {
        var item = new TestEntity { Id = Guid.NewGuid() };

        var action = async () => await _repository.UpdateAsync(item);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {item.Id}");
    }
}
