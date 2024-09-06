using ASpotifyPlaylists.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyPlaylists.Domain
{
    public class ASpotifyDbContext : DbContext
    {
        public ASpotifyDbContext(DbContextOptions<ASpotifyDbContext> options) : base(options) { }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().ToTable("Users");
            //modelBuilder.Entity<Artist>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
