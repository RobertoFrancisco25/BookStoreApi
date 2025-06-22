using BookstoreApi.Helpers;

namespace BookstoreApi.Models;

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public PaginationMetadata Metadata { get; set; }
}