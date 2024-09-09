using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace AppLibrary.Tests.LocalRepositoryTests;

public class GetPagedList : RepositoryTestBase
{
    [Test]
    public async Task GetPagedListAsync_ReturnsCorrectPagedResults()
    {
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = Repository.Items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedListAsync_WithPredicate_ReturnsCorrectPagedResults()
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
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = Repository.Items.Count;
        var pagingAsc = new PaginatedRequest(1, itemsCount, "Note asc");
        var pagingDesc = new PaginatedRequest(1, itemsCount, "Note desc");

        var resultAsc = await Repository.GetPagedListAsync(pagingAsc);
        var resultDesc = await Repository.GetPagedListAsync(pagingDesc);

        using (new AssertionScope())
        {
            resultAsc.Count.Should().Be(itemsCount);
            resultDesc.Count.Should().Be(itemsCount);
            resultAsc.Should().BeEquivalentTo(Repository.Items);
            resultDesc.Should().BeEquivalentTo(Repository.Items);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            resultAsc.Should().BeInAscendingOrder(e => e.Note, comparer);
            resultDesc.Should().BeInDescendingOrder(e => e.Note, comparer);
        }
    }
}
