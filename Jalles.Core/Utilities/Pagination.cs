namespace Jalles.Core.Utilities;

public class Pagination
{
    public Pagination(int numberOfItems, int pageIndex)
    {
        DisplayedPages = Enumerable.Empty<int>();
        Page = pageIndex <= 0 ? 1 : pageIndex;
        TotalPages = (int)Math.Ceiling(numberOfItems / (decimal)PageSize);
        GetDisplayedPages();
    }

    public int Page { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
    public int NumberOfDisplayedPages { get; set; } = 5;
    public IEnumerable<int> DisplayedPages { get; set; }

    public void GetDisplayedPages()
    {
        var numbers = new List<int>();
        for(var i = 1; i <= TotalPages; i++)
        {
            numbers.Add(i);
        }

        DisplayedPages = Page switch
        {
            <= 3 => numbers.Take(NumberOfDisplayedPages),
            _ => Page + 2 > TotalPages
                ? numbers.TakeLast(NumberOfDisplayedPages)
                : numbers.Skip(Page - 3).Take(NumberOfDisplayedPages)
        };
    }
}
