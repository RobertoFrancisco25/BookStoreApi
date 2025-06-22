using BookstoreApi.Pagination;

namespace BookstoreApi.Helpers;

public class PaginationMetadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }

    public static PaginationMetadata FromPagedList<T>(PagedList<T> pagedList) where T : class
    {
        return new PaginationMetadata()
        {
            CurrentPage = pagedList.CurrentPage,
            PageSize = pagedList.PageSize,
            TotalCount = pagedList.TotalCount,
            TotalPages = pagedList.TotalPages,
            HasPrevious = pagedList.HasPrevious,
            HasNext = pagedList.HasNext,
        };

    } 
}