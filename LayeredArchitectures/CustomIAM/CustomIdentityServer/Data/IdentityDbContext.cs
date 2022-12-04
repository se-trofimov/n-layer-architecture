using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityServer.Data;

public sealed class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserLogin> UserLogins { get; set; } = null!;
}
