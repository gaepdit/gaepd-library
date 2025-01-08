using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Domain.Repositories;

namespace AppLibrary.Tests.EfRepositoryTests.IncludePropertiesTests;

public class GetIncludeProperties : NavigationPropertiesTestBase
{
    [Test]
    public void Get_WhenEntityDoesNotExist_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var func = async () => await Repository.GetAsync(id, includeProperties: []);

        // Assert
        func.Should().ThrowAsync<EntityNotFoundException<TestEntity>>()
            .WithMessage($"Entity not found. Entity type: {typeof(TestEntity).FullName}, id: {id}");
    }

    [Test]
    public async Task WhenRequestingProperty_ReturnsEntityWithProperty()
    {
        // Arrange
        var expected = NavigationPropertyEntities[0];

        // Act
        var result = await Repository.GetAsync(expected.Id,
            includeProperties: [nameof(TestEntityWithNavigationProperties.TextRecords)]);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected);
        result.TextRecords.Count.Should().Be(expected.TextRecords.Count);
    }

    [Test]
    public async Task WhenNotRequestingProperty_ReturnsEntityWithoutProperty()
    {
        // Arrange
        var expected = NavigationPropertyEntities[0];

        // Act
        var result = await Repository.GetAsync(expected.Id, includeProperties: []);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expected, options => options.Excluding(entity => entity.TextRecords));
        result.TextRecords.Count.Should().Be(expected: 0);
    }
}
