using GaEpd.AppLibrary.Pagination;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;
using System.Globalization;

namespace GaEpd.AppLibrary.Tests.EfRepositoryTests;

public class GetPagedList
{
    private EfRepositoryTestHelper _helper = default!;

    private EfRepository _repository = default!;

    [SetUp]
    public void SetUp()
    {
        _helper = EfRepositoryTestHelper.CreateRepositoryHelper();
        _repository = _helper.GetEfRepository();
    }

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetPagedListAsync_ReturnsCorrectPagedResults()
    {
        var items = _repository.Context.Set<TestEntity>();
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await _repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedListAsync_WithPredicate_ReturnsCorrectPagedResults()
    {
        var items = _repository.Context.Set<TestEntity>();
        // Assuming this is the correct selection based on your predicate.
        var selectedItems = items.Skip(1).ToList();
        var paging = new PaginatedRequest(1, 1);
        var expectedResults = selectedItems.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await _repository.GetPagedListAsync(e => e.Id == selectedItems[0].Id, paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        await _helper.ClearTestEntityTableAsync();
        var paging = new PaginatedRequest(1, 1);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var items = _repository.Context.Set<TestEntity>();
        var paging = new PaginatedRequest(2, items.Count());

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var items = _repository.Context.Set<TestEntity>();
        var itemsCount = items.Count();
        var pagingAsc = new PaginatedRequest(1, itemsCount, "Name asc");
        var pagingDesc = new PaginatedRequest(1, itemsCount, "Name desc");

        var resultAsc = await _repository.GetPagedListAsync(pagingAsc);
        var resultDesc = await _repository.GetPagedListAsync(pagingDesc);

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
