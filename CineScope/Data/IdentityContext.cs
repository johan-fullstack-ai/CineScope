using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<CineScope.Data.ApplicationUser>(options)
{
}
