using GaEpd.AppLibrary.Pagination;
using GaEpd.AppLibrary.Tests.RepositoryHelpers;
using System.Globalization;

namespace GaEpd.AppLibrary.Tests.LocalRepositoryTests;

public class GetPagedList
{
    private LocalRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = LocalRepositoryTestHelper.GetTestRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetPagedListAsync_ReturnsCorrectPagedResults()
    {
        var paging = new PaginatedRequest(2, 1);
        var expectedResults = _repository.Items.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await _repository.GetPagedListAsync(paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task GetPagedListAsync_WithPredicate_ReturnsCorrectPagedResults()
    {
        // Assuming this is the correct selection based on your predicate.
        var selectedItems = _repository.Items.Skip(1).ToList();
        var paging = new PaginatedRequest(1, 1);
        var expectedResults = selectedItems.Skip(paging.Skip).Take(paging.Take).ToList();

        var results = await _repository.GetPagedListAsync(e => e.Id == selectedItems[0].Id, paging);

        results.Should().BeEquivalentTo(expectedResults);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        _repository.Items.Clear();
        var paging = new PaginatedRequest(1, 1);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenPagedBeyondExistingItems_ReturnsEmptyList()
    {
        var paging = new PaginatedRequest(2, _repository.Items.Count);

        var result = await _repository.GetPagedListAsync(paging);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenSorting_ReturnsSortedList()
    {
        var itemsCount = _repository.Items.Count;
        var pagingAsc = new PaginatedRequest(1, itemsCount, "Name asc");
        var pagingDesc = new PaginatedRequest(1, itemsCount, "Name desc");

        var resultAsc = await _repository.GetPagedListAsync(pagingAsc);
        var resultDesc = await _repository.GetPagedListAsync(pagingDesc);

        using (new AssertionScope())
        {
            resultAsc.Count.Should().Be(itemsCount);
            resultDesc.Count.Should().Be(itemsCount);
            resultAsc.Should().BeEquivalentTo(_repository.Items);
            resultDesc.Should().BeEquivalentTo(_repository.Items);

            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
            resultAsc.Should().BeInAscendingOrder(e => e.Name, comparer);
            resultDesc.Should().BeInDescendingOrder(e => e.Name, comparer);
        }
    }
}
