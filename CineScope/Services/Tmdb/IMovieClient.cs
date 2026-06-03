namespace CineScope.Services.Tmdb;

public interface IMovieClient
{
    public Task<TmdbSearchResponse?> SearchAsync(string query, int page, CancellationToken ct);
    public Task<TmdbMovie?> GetMovieByIdAsync(int id, CancellationToken ct);
    public Task<TmdbSearchResponse?> GetTrendingMoviesAsync(CancellationToken ct);
}


