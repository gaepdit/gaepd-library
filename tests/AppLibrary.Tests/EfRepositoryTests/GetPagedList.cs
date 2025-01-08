using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Pagination;

namespace AppLibrary.Tests.EfRepositoryTests;

public class GetPagedList : RepositoryTestBase
{
    [Test]
    public async Task GetPagedList_ReturnsCorrectPagedResults()
    {
        // Arrange
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = Repository.Context.Set<TestEntity>().Skip(paging.Skip).Take(paging.Take).ToList();

        // Act
        var results = await Repository.GetPagedListAsync(paging);

        // Assert
        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedList_WithPredicate_ReturnsCorrectPagedResults()
    {
        // Arrange
        var paging = new PaginatedRequest(1, 1);

        // Assuming this is the correct selection based on your predicate.
        var selectedItems = Repository.Context.Set<TestEntity>().Skip(1).ToList();
        var expectedResults = selectedItems.Skip(paging.Skip).Take(paging.Take).ToList();

        // Act
        var results = await Repository.GetPagedListAsync(e => e.Id == selectedItems[0].Id, paging);

        // Assert
        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        await Helper.ClearTableAsync<TestEntity>();
        var paging = new PaginatedRequest(1, 1);

        // Act
        var result = await Repository.GetPagedListAsync(paging);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        // Arrange
        var items = Repository.Context.Set<TestEntity>();
        var paging = new PaginatedRequest(2, items.Count());

        // Act
        var result = await Repository.GetPagedListAsync(paging);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenAscSorting_ReturnsAscSortedList()
    {
        // Arrange
        var items = Repository.Context.Set<TestEntity>().OrderBy(entity => entity.Note);
        var itemsCount = items.Count();
        var paging = new PaginatedRequest(1, itemsCount, "Note");

        // Act
        var result = await Repository.GetPagedListAsync(paging);

        // Assert
        result.Should().BeEquivalentTo(items, options => options.WithStrictOrdering());
    }

    [Test]
    public async Task GivenDescSorting_ReturnsDescSortedList()
    {
        // Arrange
        var items = Repository.Context.Set<TestEntity>().OrderByDescending(entity => entity.Note);
        var itemsCount = items.Count();
        var paging = new PaginatedRequest(1, itemsCount, "Note desc");

        // Act
        var result = await Repository.GetPagedListAsync(paging);

        // Assert
        result.Should().BeEquivalentTo(items, options => options.WithStrictOrdering());
    }
}
