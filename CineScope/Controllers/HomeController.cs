using CineScope.Models.ViewModels;
using CineScope.Services.Tmdb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CineScope.Controllers;

public class HomeController : Controller
{
    private readonly IMovieClient _movieClient;

    public HomeController(IMovieClient movieClient)
    {
        _movieClient = movieClient;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await _movieClient.GetTrendingMoviesAsync(ct);

        // Trending always returns a single page, so pagination reflects that reality.
        var display = new PagedMoviesDisplay
        {
            Movies     = (response?.Results ?? []).Select(m => m.ToMovie()).ToList(),
            Pagination = PaginationInfo.Build(
                             currentPage: response?.Page ?? 1,
                             totalPages:  response?.TotalPages ?? 1)
        };

        return View(display);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
