using GaEpd.AppLibrary.GuardClauses;
using System.Text.Json.Serialization;

namespace GaEpd.AppLibrary.Pagination;

public class PaginatedResult<T> : IPaginatedResult<T>
    where T : class
{
    public PaginatedResult(IEnumerable<T> items, int totalCount, IPaginatedRequest paging)
    {
        TotalCount = Guard.NotNegative(totalCount);
        PageNumber = paging.PageNumber;
        PageSize = paging.PageSize;

        var itemsList = new List<T>();
        itemsList.AddRange(items);
        Items = itemsList;
    }

    public int TotalCount { get; }
    public int PageSize { get; }
    public int PageNumber { get; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int CurrentCount => Items.Count;

    [JsonIgnore]
    public int FirstItemIndex => Math.Min(PageSize * (PageNumber - 1) + 1, TotalCount);

    [JsonIgnore]
    public int LastItemIndex => Math.Min(PageSize * PageNumber, TotalCount);

    [JsonIgnore]
    public bool HasPreviousPage => PageNumber > 1;

    [JsonIgnore]
    public int PreviousPageNumber => Math.Max(1, PageNumber - 1);

    [JsonIgnore]
    public bool HasNextPage => PageNumber < TotalPages;

    [JsonIgnore]
    public int NextPageNumber => Math.Min(PageNumber + 1, TotalPages);

    public IList<T> Items { get; }
}
