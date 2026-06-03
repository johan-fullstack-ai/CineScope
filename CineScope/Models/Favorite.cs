using CineScope.Data;
using Microsoft.AspNetCore.Identity;

namespace CineScope.Models;

public class Favorite
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
}

