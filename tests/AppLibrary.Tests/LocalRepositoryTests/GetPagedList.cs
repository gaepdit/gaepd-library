using GaEpd.AppLibrary.Pagination;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class GetPagedList : RepositoryTestBase
{
    [Test]
    public async Task GetPagedList_ReturnsCorrectPagedResults()
    {
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = Repository.Items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedList_WithPredicate_ReturnsCorrectPagedResults()
    {
        // Assuming this is the correct selection based on your predicate.
        var selectedItems = Repository.Items.Skip(1).ToList();
        var paging = new PaginatedRequest(1, 1);
        var expectedResults = selectedItems.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(e => e.Id == selectedItems[0].Id, paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        Repository.Items.Clear();
        var paging = new PaginatedRequest(1, 1);

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(2, Repository.Items.Count);

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenAscSorting_ReturnsAscSortedList()
    {
        // Arrange
        var items = Repository.Items.OrderBy(entity => entity.Note).ToList();
        var itemsCount = items.Count;
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
        var items = Repository.Items.OrderByDescending(entity => entity.Note).ToList();
        var itemsCount = items.Count;
        var paging = new PaginatedRequest(1, itemsCount, "Note desc");

        // Act
        var result = await Repository.GetPagedListAsync(paging);

        // Assert
        result.Should().BeEquivalentTo(items, options => options.WithStrictOrdering());
    }
}
