namespace GaEpd.AppLibrary.Pagination;

/// <summary>
/// An interface for requesting a sorted and paged result list.
/// </summary>
public interface IPaginatedRequest
{
    /// <summary>
    /// The size of each page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// The page number requested.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Sorting definition. Should be a comma-separated list of properties with sort direction.
    /// E.g., "Name ASC, Date DESC" ("ASC" is optional).
    /// </summary>
    /// <remarks>See https://dynamic-linq.net/basic-simple-query#ordering-results for sorting spec.</remarks>
    string Sorting { get; }

    /// <summary>
    /// The number of results to skip.
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// The maximum number of results to return (equal to page size).
    /// </summary>
    int Take { get; }
}
