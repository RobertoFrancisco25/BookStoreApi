using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Parameters;

public abstract class PaginationParameters
{
    const int maxPageSize = 50;
    /// <summary>
    /// The page number to retrieve.
    /// </summary>
    [Range(1,int.MaxValue,ErrorMessage = "PageNumber must be at least 1")]
    public int PageNumber { get; set; } = 1;
    private int _pageSize = maxPageSize;
    /// <summary>
    /// The number of items per page.
    /// </summary>
    [Range(1, maxPageSize,ErrorMessage = "PageSize must be at least 1")]
    public int PageSize
    {
        get {return _pageSize; }
        set {_pageSize = (value > maxPageSize) ? maxPageSize : value; }
    }
}
