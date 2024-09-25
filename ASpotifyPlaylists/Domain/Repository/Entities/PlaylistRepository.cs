using ASpotifyPlaylists.Domain.Entities;
using ASpotifyPlaylists.Domain.Repository.Abstract;
using ASpotifyPlaylists.Dto;
using Microsoft.EntityFrameworkCore;

namespace ASpotifyPlaylists.Domain.Repository.Entities
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ASpotifyDbContext _context;
        public PlaylistRepository(ASpotifyDbContext context) 
        {
            _context = context;
        }
        public async Task<List<Playlist>> GetPopular()
        {
            var todayDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
                .TotalMilliseconds - 1000000000;
            return await _context.Playlists
                .Where(x => x.Types == PlaylistTypes.Album)
                .Where(x => x.CreatedDate > todayDate)
                .OrderBy(x => Guid.NewGuid())
                .Take(6)
                .ToListAsync();
        }
    }
}
