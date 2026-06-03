using CineScope.Data;
using CineScope.Models.ViewModels;
using CineScope.Services.Tmdb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineScope.Controllers;

[Authorize(Roles = "Admin")]
public class TmdbController : Controller
{
    private readonly IMovieClient _movieClient;
    private readonly MyDbContext _context;

    public TmdbController(IMovieClient movieClient, MyDbContext context)
    {
        _movieClient = movieClient;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Search(string? query, int page = 1, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new PagedMoviesDisplay());
        }

        var response = await _movieClient.SearchAsync(query, page, ct);

        var display = new PagedMoviesDisplay
        {
            Query      = query,
            Movies     = (response?.Results ?? []).Select(m => m.ToMovie()).ToList(),
            Pagination = PaginationInfo.Build(
                             currentPage: response?.Page ?? 1,
                             totalPages:  response?.TotalPages ?? 1)
        };

        return View(display);
    }

    [HttpPost]
    public async Task<IActionResult> Import(int id, CancellationToken ct)
    {
        var tmdbMovie = await _movieClient.GetMovieByIdAsync(id, ct);
        if (tmdbMovie == null)
        {
            TempData["Error"] = "Failed to fetch movie from TMDB.";
            return RedirectToAction(nameof(Search));
        }

        var movie = tmdbMovie.ToMovie();
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync(ct);

        TempData["Success"] = $"'{movie.Title}' has been imported successfully.";
        return RedirectToAction(nameof(Search));
    }
}
