namespace GaEpd.AppLibrary.Pagination;

public class PaginatedRequest(int pageNumber, int pageSize, string sorting = "") : IPaginatedRequest
{
    public int PageSize { get; } = Guard.Positive(pageSize);
    public int PageNumber { get; } = Guard.Positive(pageNumber);
    public string Sorting { get; } = sorting;

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}
