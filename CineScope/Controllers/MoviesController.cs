using CineScope.Data;
using CineScope.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Controllers;


public class MoviesController : Controller
{
    private readonly MyDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public MoviesController(MyDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Movies
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? searchString)
    {
        var movies = from m in _context.Movies select m;
        if (!string.IsNullOrWhiteSpace(searchString))
            movies = movies.Where(m => m.Title != null && m.Title.Contains(searchString));
        ViewData["CurrentFilter"] = searchString;
        return View(await movies.ToListAsync());
    }

    // GET: Movies/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        string? userId = user?.Id;

        bool isFavorite = false;

        if (userId != null)
        {
            isFavorite = await _context.Favorites
                .AnyAsync(f => f.MovieId == id && f.UserId == userId);
        }

        ViewBag.IsFavorite = isFavorite; // Pass the favorite status to the view

        return View(movie);
    }

    // GET: Movies/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id,Title,Genre,ReleaseYear,Rating,Duration,PosterUrl,Description")] Movie movie)
    {
        _context.Add(movie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();

        return View(movie);
    }

    // POST: Movies/Edit/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ReleaseYear,Rating,Duration,PosterUrl,Description")] Movie movie)
    {
        if (id != movie.Id) return NotFound();

        try
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Movies.Any(e => e.Id == movie.Id))
                return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Movies/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();

        return View(movie);
    }

    // POST: Movies/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
