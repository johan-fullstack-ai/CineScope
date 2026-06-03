using System.Globalization;

namespace CineScope.Services.Tmdb;

//Registers a named HttpClient with BaseAddress and timeout, and binds IMovieClient to the implementation. Use builder.Services.AddTmdbClient(builder.Configuration) in Program.cs.
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTmdbClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("tmdb", client =>
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            int timeout = int.Parse(configuration["TMDB:TimeoutSeconds"] ?? "10", CultureInfo.InvariantCulture);
            client.Timeout = TimeSpan.FromSeconds(timeout);
        });

        services.AddScoped<IMovieClient, TmdbMovieClient>();
        return services;
    }
}