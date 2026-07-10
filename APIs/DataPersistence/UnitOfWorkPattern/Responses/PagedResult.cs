namespace RestfulApi.Responses;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    private PagedResult() { }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int totalCount,
        int page,
        int pageSize
    )
    {
        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
        };
    }
}
