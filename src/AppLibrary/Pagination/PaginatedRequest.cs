using GaEpd.AppLibrary.GuardClauses;

namespace GaEpd.AppLibrary.Pagination;

public class PaginatedRequest : IPaginatedRequest
{
    public PaginatedRequest(int pageNumber, int pageSize, string sorting = "")
    {
        PageNumber = Guard.Positive(pageNumber);
        PageSize = Guard.Positive(pageSize);
        Sorting = sorting;
    }

    public int PageSize { get; }
    public int PageNumber { get; }
    public string Sorting { get; }

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}
