namespace CineScope.Models.ViewModels;

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public int PreviousPage => CurrentPage - 1;
    public int NextPage => CurrentPage + 1;

    public List<int> PageNumbers { get; set; } = [];

    /// <summary>
    /// Builds a <see cref="PaginationInfo"/> with a sliding window of page numbers
    /// centred on <paramref name="currentPage"/> (±2 neighbours).
    /// </summary>
    public static PaginationInfo Build(int currentPage, int totalPages, int windowRadius = 2)
    {
        int start = Math.Max(1, currentPage - windowRadius);
        int end   = Math.Min(totalPages, currentPage + windowRadius);

        return new PaginationInfo
        {
            CurrentPage = currentPage,
            TotalPages  = totalPages,
            PageNumbers = Enumerable.Range(start, end - start + 1).ToList()
        };
    }
}
