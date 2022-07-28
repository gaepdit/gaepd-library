using GaEpd.Library.Utilities;
using System.Text.Json.Serialization;

namespace GaEpd.Library.Pagination;

public class PaginatedResult<T> : IPaginatedResult<T>
    where T : class
{
    public PaginatedResult(IEnumerable<T> items, int totalCount, IPaginatedRequest paging)
    {
        TotalCount = Guard.NotNegative(totalCount, nameof(totalCount));
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
    public bool HasNextPage => PageNumber < TotalPages;

    public IList<T> Items { get; }
}
