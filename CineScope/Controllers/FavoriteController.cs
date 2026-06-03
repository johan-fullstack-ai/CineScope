using CineScope.Data;
using CineScope.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineScope.Controllers;

// Authorizes members and admins to manage their favorite movies, because it requires user authentication to associate favorites with a specific user account.
[Authorize]
public class FavoriteController : Controller
{
    private readonly MyDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoriteController(MyDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Add(int movieId, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var exists = await _context.Favorites.AnyAsync(f => f.MovieId == movieId && f.UserId == user.Id, ct);
        if (!exists)
        {
            _context.Favorites.Add(new Favorite
            {
                MovieId = movieId,
                UserId = user.Id
            });

            await _context.SaveChangesAsync(ct);
        }

        return RedirectToAction("Details", "Movies", new { id = movieId });
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int movieId, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.MovieId == movieId && f.UserId == user.Id, ct);
        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync(ct);
        }

        return RedirectToAction("Details", "Movies", new { id = movieId });
    }
}