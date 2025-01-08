using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Pagination;
using Microsoft.EntityFrameworkCore;

namespace AppLibrary.Tests.EfRepositoryTests.IncludePropertiesTests;

public class GetPagedListIncludeProperties : NavigationPropertiesTestBase
{
    private const string TextRecordsName = nameof(TestEntityWithNavigationProperties.TextRecords);

    [Test]
    public async Task GetPagedList_WithNoIncludedProperties_ReturnsWithoutProperties()
    {
        // Arrange
        var paging = new PaginatedRequest(1, 10);
        var expectedResults = await Repository.Context.Set<TestEntityWithNavigationProperties>()
            .Include(entity => entity.TextRecords)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync();

        // Act
        var results = await Repository.GetPagedListAsync(paging,
            includeProperties: []);

        // Assert
        using var scope = new AssertionScope();
        results.Should().NotBeEquivalentTo(expectedResults);
        results.Should().BeEquivalentTo(expectedResults, options => options.Excluding(entity => entity.TextRecords));
    }

    [Test]
    public async Task GetPagedList_WithIncludedProperties_ReturnsWithProperties()
    {
        // Arrange
        var paging = new PaginatedRequest(1, 10);
        var expectedResults = await Repository.Context.Set<TestEntityWithNavigationProperties>()
            .Include(entity => entity.TextRecords)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync();

        // Act
        var results = await Repository.GetPagedListAsync(paging,
            includeProperties: [TextRecordsName]);

        // Assert
        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedList_WithPredicate_WithNoIncludedProperties_ReturnsWithoutProperties()
    {
        // Arrange
        var excludedName = NavigationPropertyEntities[0].Name;
        var paging = new PaginatedRequest(1, 10);
        var expectedResults = await Repository.Context.Set<TestEntityWithNavigationProperties>()
            .Include(entity => entity.TextRecords)
            .Where(entity => entity.Name != excludedName)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync();

        // Act
        var results = await Repository.GetPagedListAsync(e => e.Name != excludedName, paging,
            includeProperties: []);

        // Assert
        using var scope = new AssertionScope();
        results.Should().NotBeEquivalentTo(expectedResults);
        results.Should().BeEquivalentTo(expectedResults, options => options.Excluding(entity => entity.TextRecords));
    }

    [Test]
    public async Task GetPagedList_WithPredicate_WithIncludedProperties_ReturnsWithProperties()
    {
        // Arrange
        var excludedName = NavigationPropertyEntities[0].Name;
        var paging = new PaginatedRequest(1, 10);
        var expectedResults = await Repository.Context.Set<TestEntityWithNavigationProperties>()
            .Include(entity => entity.TextRecords)
            .Where(entity => entity.Name != excludedName)
            .Skip(paging.Skip).Take(paging.Take).ToListAsync();

        // Act
        var results = await Repository.GetPagedListAsync(e => e.Name != excludedName, paging,
            includeProperties: [TextRecordsName]);

        // Assert
        results.Should().BeEquivalentTo(expectedResults);
    }
}
