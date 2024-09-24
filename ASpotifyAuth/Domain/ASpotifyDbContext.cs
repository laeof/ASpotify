using ASpotifyAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyAuth.Domain
{
    public class ASpotifyDbContext : DbContext
    {
        public ASpotifyDbContext(DbContextOptions<ASpotifyDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
