namespace ProjectF.Core.Models;

public class PagedResult<T>
{
    public PagedResult(ICollection<T> allItems, long pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        
        var (skip, take) = GetSkipTake(PageNumber, PageSize);
        TotalItems = allItems.Count;
        Items = allItems.Skip(skip).Take(take).ToList();

        if (PageSize > 0)
        {
            TotalPages = (long)Math.Ceiling(TotalItems / (decimal)PageSize);
        }
        else
        {
            TotalPages = 1;
        }
    }

    public long PageNumber { get; }
    public int PageSize { get; }
    public long TotalPages { get; }
    public long TotalItems { get; }
    public ICollection<T>? Items { get; }

    /// <summary>
    /// Calculates the skip size based on the paged parameters specified
    /// </summary>
    /// <remarks>
    /// Returns 0 if the page number or page size is zero
    /// </remarks>
    private static (int Skip, int Take) GetSkipTake(long pageNumber, int pageSize)
    {
        if (pageNumber > 0 && pageSize > 0)
        {
            return (Convert.ToInt32((pageNumber - 1) * pageSize), pageSize);
        }

        return (0, pageSize);
    }
}