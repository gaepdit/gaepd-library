using GaEpd.AppLibrary.Pagination;
using GaEpd.AppLibrary.Tests.EntityHelpers;
using System.Globalization;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class GetPagedList : EfRepositoryTestBase
{
    [Test]
    public async Task GetPagedListAsync_ReturnsCorrectPagedResults()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedListAsync_WithPredicate_ReturnsCorrectPagedResults()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        // Assuming this is the correct selection based on your predicate.
        var selectedItems = items.Skip(1).ToList();
        var paging = new PaginatedRequest(1, 1);
        var expectedResults = selectedItems.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(e => e.Id == selectedItems[0].Id, paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await Helper.ClearTestEntityTableAsync();
        var paging = new PaginatedRequest(1, 1);

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        var paging = new PaginatedRequest(2, items.Count());

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var items = Repository.Context.Set<DerivedEntity>();
        var itemsCount = items.Count();
        var pagingAsc = new PaginatedRequest(1, itemsCount, "Name asc");
        var pagingDesc = new PaginatedRequest(1, itemsCount, "Name desc");

        var resultAsc = await Repository.GetPagedListAsync(pagingAsc);
        var resultDesc = await Repository.GetPagedListAsync(pagingDesc);

        using (new AssertionScope())
        {
            resultAsc.Count.Should().Be(itemsCount);
            resultDesc.Count.Should().Be(itemsCount);
            resultAsc.Should().BeEquivalentTo(items);
            resultDesc.Should().BeEquivalentTo(items);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            resultAsc.Should().BeInAscendingOrder(e => e.Name, comparer);
            resultDesc.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
