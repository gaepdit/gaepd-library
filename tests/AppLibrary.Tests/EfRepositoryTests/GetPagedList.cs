using AppLibrary.Tests.TestEntities;
using GaEpd.AppLibrary.Pagination;
using System.Globalization;

namespace AppLibrary.Tests.EfRepositoryTests;

public class GetPagedList : RepositoryTestBase
{
    [Test]
    public async Task GetPagedList_ReturnsCorrectPagedResults()
    {
        var items = Repository.Context.Set<TestEntity>();
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await Repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedList_WithPredicate_ReturnsCorrectPagedResults()
    {
        var items = Repository.Context.Set<TestEntity>();
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
        await Helper.ClearTableAsync<TestEntity>();
        var paging = new PaginatedRequest(1, 1);

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var items = Repository.Context.Set<TestEntity>();
        var paging = new PaginatedRequest(2, items.Count());

        var result = await Repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var items = Repository.Context.Set<TestEntity>();
        var itemsCount = items.Count();
        var pagingAsc = new PaginatedRequest(1, itemsCount, "Note asc");
        var pagingDesc = new PaginatedRequest(1, itemsCount, "Note desc");

        var resultAsc = await Repository.GetPagedListAsync(pagingAsc);
        var resultDesc = await Repository.GetPagedListAsync(pagingDesc);

        using (new AssertionScope())
        {
            resultAsc.Count.Should().Be(itemsCount);
            resultDesc.Count.Should().Be(itemsCount);
            resultAsc.Should().BeEquivalentTo(items);
            resultDesc.Should().BeEquivalentTo(items);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            resultAsc.Should().BeInAscendingOrder(e => e.Note, comparer);
            resultDesc.Should().BeInDescendingOrder(e => e.Note, comparer);
        }
    }
}
