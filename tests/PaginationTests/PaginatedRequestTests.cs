using FluentAssertions;
using GaEpd.Library.Pagination;

namespace GaEpd.Library.Tests.PaginationTests;

public class PaginatedRequestTests
{
    [Test]
    public void ReturnsWithPagingGivenValidInputs()
    {
        const int pageNumber = 2;
        const int pageSize = 10;
        const string sorting = "Name";

        var pagination = new PaginatedRequest(pageNumber, pageSize, sorting);

        Assert.Multiple(() =>
        {
            pagination.PageSize.Should().Be(pageSize);
            pagination.PageNumber.Should().Be(pageNumber);
            pagination.Sorting.Should().Be(sorting);
            pagination.Skip.Should().Be((pageNumber - 1) * pageSize);
            pagination.Take.Should().Be(pageSize);
        });
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageSize()
    {
        const int pageNumber = 2;
        const int pageSize = 0;

        var action = () => new PaginatedRequest(pageNumber, pageSize);
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(nameof(pageSize));
    }

    [Test]
    public void ThrowsExceptionGivenZeroPageNumber()
    {
        const int pageNumber = 0;
        const int pageSize = 20;

        var action = () => new PaginatedRequest(pageNumber, pageSize);
        action.Should().Throw<ArgumentException>()
            .And.ParamName.Should().Be(nameof(pageNumber));
    }
}
