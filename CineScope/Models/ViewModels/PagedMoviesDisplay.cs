namespace CineScope.Models.ViewModels;

public class PagedMoviesDisplay
{
    public List<Movie> Movies { get; set; } = [];
    public PaginationInfo Pagination { get; set; } = new();
    public string Query { get; set; } = "";
}
