namespace GaEpd.AppLibrary.Pagination;

/// <summary>
/// An interface for returning a sorted and paged result list.
/// </summary>
public interface IPaginatedResult
{
    int TotalCount { get; }
    int PageSize { get; }
    int PageNumber { get; }

    int TotalPages { get; }
    int FirstItemIndex { get; }
    int LastItemIndex { get; }
    int CurrentCount { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}

public interface IPaginatedResult<T> : IPaginatedResult
    where T : class
{
    public IList<T> Items { get; }
}
