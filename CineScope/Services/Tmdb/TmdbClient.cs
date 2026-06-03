using System.Net.Http.Headers;

namespace CineScope.Services.Tmdb;

//TmdbClient handles HTTP calls, sets the Authorization header with the Bearer token, builds relative endpoints, and deserializes the responses into models.
public class TmdbMovieClient : IMovieClient
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly string _apiKey;
    private readonly ILogger<TmdbMovieClient> _log;

    public TmdbMovieClient(IHttpClientFactory httpFactory, IConfiguration cfg, ILogger<TmdbMovieClient> log)
    {
        _httpFactory = httpFactory;
        ArgumentNullException.ThrowIfNull(cfg);
        _apiKey = cfg["TMDB:ApiKey"]
            ?? throw new InvalidOperationException("Missing configuration value: TMDB:ApiKey");
        _log = log;
    }

    public async Task<TmdbSearchResponse?> SearchAsync(string query, int page, CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("tmdb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var url = $"search/movie?query={Uri.EscapeDataString(query)}&page={page}";
        var res = await client.GetAsync(url, ct);

        if (!res.IsSuccessStatusCode)
        {
            _log.LogWarning("TMDB API search failed: {Status}", res.StatusCode);
            return null;
        }

        return await res.Content.ReadFromJsonAsync<TmdbSearchResponse>(cancellationToken: ct);
    }

    public async Task<TmdbMovie?> GetMovieByIdAsync(int id, CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("tmdb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var url = $"movie/{id}";
        var res = await client.GetAsync(url, ct);

        if (!res.IsSuccessStatusCode)
        {
            _log.LogWarning("TMDB get movie by {Id} failed: {Status}", id, res.StatusCode);
            return null;
        }

        return await res.Content.ReadFromJsonAsync<TmdbMovie>(cancellationToken: ct);
    }

    public async Task<TmdbSearchResponse?> GetTrendingMoviesAsync(CancellationToken ct)
    {
        var client = _httpFactory.CreateClient("tmdb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var url = "trending/movie/week";
        var res = await client.GetAsync(url, ct);

        if (!res.IsSuccessStatusCode)
        {
            _log.LogWarning("TMDB trending movies failed: {Status}", res.StatusCode);
            return null;
        }

        return await res.Content.ReadFromJsonAsync<TmdbSearchResponse>(cancellationToken: ct);
    }
}

