using System.ComponentModel.DataAnnotations.Schema;

namespace CineScope.Models;

public class Movie
{
    public int Id { get; set; }

    /// <summary>
    /// Carries the originating TMDB id for search results so the Import action
    /// can fetch full details. Not persisted — EF ignores this column.
    /// </summary>
    [NotMapped]
    public int TmdbId { get; set; }

    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? ReleaseYear { get; set; }
    public string? Rating { get; set; }
    public string? Duration { get; set; }
    public string? PosterUrl { get; set; }
    public string? Description { get; set; }
}