namespace ProjectF.Core.Models;

public class PagedResult<T>
{
    public PagedResult(long totalItems, long pageNumber, long pageSize)
    {
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;

        if (pageSize > 0)
        {
            TotalPages = (long)Math.Ceiling(totalItems / (decimal)pageSize);
        }
        else
        {
            TotalPages = 1;
        }
    }

    public long PageNumber { get; private set; }
    public long PageSize { get; private set; }
    public long TotalPages { get; private set; }
    public long TotalItems { get; private set; }
    public IEnumerable<T>? Items { get; set; }

    /// <summary>
    ///     Calculates the skip size based on the paged parameters specified
    /// </summary>
    /// <remarks>
    ///     Returns 0 if the page number or page size is zero
    /// </remarks>
    public int GetSkipSize()
    {
        if (PageNumber > 0 && PageSize > 0)
        {
            return Convert.ToInt32((PageNumber - 1) * PageSize);
        }

        return 0;
    }
}
