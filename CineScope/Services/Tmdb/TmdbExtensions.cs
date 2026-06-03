using CineScope.Models;
using System.Globalization;

namespace CineScope.Services.Tmdb;

public static class TmdbExtensions
{
    /// <summary>
    /// Converts/extends a TmdbMovie to Movie entity.
    /// Security checks and fallback values ​​included.
    /// </summary>
    public static Movie ToMovie(this TmdbMovie tmdb)
    {
        ArgumentNullException.ThrowIfNull(tmdb);

        // Genre: build a comma-separated string of genre names
        string genreString = tmdb.Genres != null && tmdb.Genres.Count > 0
            ? string.Join(", ", tmdb.Genres
                .Select(g => g.Name?.Trim())
                .Where(n => !string.IsNullOrEmpty(n)))
            : string.Empty;


        string? releaseYear = null;
        if (!string.IsNullOrWhiteSpace(tmdb.ReleaseDate))
        {
            var parts = tmdb.ReleaseDate.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && int.TryParse(parts[0], out int y))
            {
                releaseYear = y.ToString(CultureInfo.InvariantCulture);
            }
        }

        string? rating = tmdb.VoteAverage?.ToString("F1", CultureInfo.InvariantCulture);


        string? duration = tmdb.Runtime?.ToString(CultureInfo.InvariantCulture);

        string? posterUrl = BuildPosterUrl(tmdb);

        var description = tmdb.Overview?.Trim();

        return new Movie
        {
            TmdbId = tmdb.Id,
            Title = tmdb.Title?.Trim() ?? string.Empty,
            Genre = genreString,
            ReleaseYear = releaseYear,
            Rating = rating,
            Duration = duration,
            PosterUrl = posterUrl,
            Description = description
        };
    }

    public static string? BuildPosterUrl(TmdbMovie tmdb)
    {
        // PosterUrl: build full URL if poster_path exists
        string? posterUrl = null;
        if (!string.IsNullOrWhiteSpace(tmdb.PosterPath))
        {
            posterUrl = $"https://image.tmdb.org/t/p/w500{tmdb.PosterPath}";
        }

        return posterUrl;
    }
}

